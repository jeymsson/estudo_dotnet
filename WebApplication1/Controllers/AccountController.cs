using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
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