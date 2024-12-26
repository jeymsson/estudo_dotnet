using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Extensions;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IPortfolioRepository portfolioRepo) {
            this._userManager = userManager;
            this._portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> getUserPortfolio() {
            var user = User.getUsername();
            var appUser = await this._userManager.FindByNameAsync(user);
            var userPortifolio = this._portfolioRepo.getUserPortfolio(appUser);
            return Ok(userPortifolio);
        }
    }
}