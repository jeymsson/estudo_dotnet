using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Custom.Attributes;
using WebApplication1.Data;
using WebApplication1.Dto.Portfolio;
using WebApplication1.Extensions;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [CustomAuthorize]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IStockRepository stockRepo;
        public PortfolioController(UserManager<AppUser> userManager, IPortfolioRepository portfolioRepo, IStockRepository stockRepo) {
            this._userManager = userManager;
            this._portfolioRepo = portfolioRepo;
            this.stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> getUserPortfolio() {
            var user = User.getUsername();
            var appUser = await this._userManager.FindByNameAsync(user);
            var userPortfolio = await this._portfolioRepo.getUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        public async Task<IActionResult> addStock([FromBody] CreatePortfolioRequestDto createPortfolioRequestDto) {
            var user = User.getUsername();
            var appUser = await this._userManager.FindByNameAsync(user);
            var stock = await this.stockRepo.findStockByName(createPortfolioRequestDto.Symbol);

            if (stock == null) {
                return BadRequest("Stock not found");
            }

            var havePortfolio = await this._portfolioRepo.findPortfolioByStockAsync(stock);

            if (havePortfolio != null) {
                return BadRequest("Stock already in portfolio");
            }

            var portfolio = new Portfolio {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };

            await this._portfolioRepo.createAsync(portfolio);

            return Ok(portfolio);
        }

        [HttpDelete]
        public async Task<IActionResult> deleteStock([FromQuery] int id) {
            var user = User.getUsername();
            var appUser = await this._userManager.FindByNameAsync(user);

            var portfolio = await this._portfolioRepo.findPortfolioByIdAsync(appUser.Id, id);

            if (portfolio == null) {
                return BadRequest("Portfolio not found");
            }

            if (portfolio.AppUserId != appUser.Id) {
                return Unauthorized("Unauthorized, you are not the owner of this portfolio");
            }

            await this._portfolioRepo.deletePortfolioAsync(portfolio);

            return Ok(portfolio);
        }
    }
}