using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> getAllAsync(QueryObject query);
        Task<Stock?> FindAsync(int id);
        Task<Stock> AddAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, Stock stockModel);
        Task<Stock> DeleteAsync(Stock stockModel);
        Task<bool> stockExists(int stockId);
        Task<Stock> findStockByName(string symbol);
    }
}