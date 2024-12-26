using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface IPortfolioRepository
    {
        public Task<List<Stock>> getUserPortfolio(AppUser user);
        public Task<Portfolio> createAsync(Portfolio portfolio);
        public Task<Portfolio> findPortfolioByStockAsync(Stock stock);
        public Task<Portfolio> findPortfolioByIdAsync(string user_id, int stock_id);
        public Task<Portfolio> deletePortfolioAsync(Portfolio portfolio);
    }
}