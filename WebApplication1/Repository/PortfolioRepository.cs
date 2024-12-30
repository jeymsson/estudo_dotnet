using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortfolioRepository(ApplicationDbContext context) {
            this._context = context;
        }

        public async Task<List<Stock>> getUserPortfolio(AppUser user)
        {
            return await this._context.Portfolios
                .Where(p => p.AppUserId == user.Id)
                .Select(stock => new Stock {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap,
                })
                .ToListAsync();
        }

        public async Task<Portfolio> createAsync(Portfolio portfolio)
        {
            await this._context.Portfolios.AddAsync(portfolio);
            await this._context.SaveChangesAsync();
            return portfolio;
        }

        public Task<Portfolio> findPortfolioByStockAsync(Stock stock)
        {
            return this._context.Portfolios.FirstOrDefaultAsync(p => p.StockId == stock.Id);
        }

        public Task<Portfolio> findPortfolioByIdAsync(string userId, int id)
        {
            return this._context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == userId.ToString() && p.StockId == id);
        }

        public Task<Portfolio> deletePortfolioAsync(Portfolio portfolio)
        {
            this._context.Portfolios.Remove(portfolio);
            this._context.SaveChanges();
            return Task.FromResult(portfolio);
        }
    }
}