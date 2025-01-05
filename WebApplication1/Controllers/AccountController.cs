using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dto.Account;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signinManager)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._signinManager = signinManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDto login) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (login.Username == null || login.Password == null)
                return Unauthorized("Username and password are required");

            var user = await this._userManager.Users.FirstOrDefaultAsync(x => x.UserName == login.Username);

            if(user == null)
                return Unauthorized("Invalid username");

            var result = await this._signinManager.CheckPasswordSignInAsync(user, login.Password, false);

            if(!result.Succeeded)
                return Unauthorized("Invalid password");

            return Ok(
                new NewUserDto {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = this._tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterDto register) {
            try {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser {
                    UserName = register.Username,
                    Email = register.Email
                };

                var createUser = await this._userManager.CreateAsync(appUser, register.Password);

                if(createUser.Succeeded) {
                    var roleResult = await this._userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded) {
                        return Ok( new NewUserDto {
                            Username = appUser.UserName,
                            Email = appUser.Email,
                            Token = this._tokenService.CreateToken(appUser)
                        } );
                    } else {
                        return StatusCode(500, roleResult.Errors);
                    }
                } else {
                    return StatusCode(500, createUser.Errors);
                }
            } catch (Exception e) {
                return StatusCode(500, e);
            }
        }

    }
}