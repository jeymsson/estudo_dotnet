using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data.Seeds
{
    public class StockSeed
    {
        public StockSeed(ModelBuilder builder){
            List<Stock> stocks = new List<Stock> {
                new Stock {
                    Id = 1,
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Purchase = 100,
                    LastDiv = 0.82m,
                    Industry = "Technology",
                    MarketCap = 2000000000,
                },
                new Stock {
                    Id = 2,
                    Symbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Purchase = 200,
                    LastDiv = 1.56m,
                    Industry = "Technology",
                    MarketCap = 2000000000,
                }
            };
            builder.Entity<Stock>().HasData(stocks);
        }
    }
}