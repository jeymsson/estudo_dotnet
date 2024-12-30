using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context) {
            this._context = context;
        }

        public async Task<List<Stock>> getAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.Comments).AsQueryable();
            if(!string.IsNullOrEmpty(query.Symbol)) {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if(!string.IsNullOrEmpty(query.CompanyName)) {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if(!string.IsNullOrEmpty(query.SortBy)) {
                if(query.SortBy == "symbol") {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                if(query.SortBy == "companyName") {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            var skip = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skip).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock> AddAsync(Stock stockModel)
        {
            await this._context.AddAsync(stockModel);
            await this._context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stockModel)
        {
            var stock = await this.FindAsync(id);
            stock.Symbol = stockModel.Symbol;
            stock.CompanyName = stockModel.CompanyName;
            stock.Purchase = stockModel.Purchase;
            stock.LastDiv = stockModel.LastDiv;
            stock.Industry = stockModel.Industry;
            stock.MarketCap = stockModel.MarketCap;
            await this._context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> FindAsync(int id)
        {
            return await this._context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock> DeleteAsync(Stock stockModel)
        {
            var stock = this._context.Remove(stockModel);
            await this._context.SaveChangesAsync();
            return stock.Entity;
        }

        public Task<bool> stockExists(int stockId) {
            return this._context.Stock.AnyAsync(s => s.Id == stockId);
        }

        public async Task<Stock> findStockByName(string symbol)
        {
            return await this._context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }
}