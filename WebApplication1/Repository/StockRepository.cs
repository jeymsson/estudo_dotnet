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
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context) {
            this._context = context;
        }

        public async Task<List<Stock>> getAllAsync()
        {
            return await _context.Stock.Include(c => c.Comments).ToListAsync();
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
            if(stock == null) {
                return null;
            }
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
    }
}