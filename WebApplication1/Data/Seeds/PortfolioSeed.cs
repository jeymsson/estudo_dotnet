using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data.Seeds
{
    public class PortfolioSeed
    {
        public PortfolioSeed(ModelBuilder builder)
        {
            List<Portfolio> portfolios = new List<Portfolio> {
                new Portfolio{
                    AppUserId = "1",
                    StockId = 1,
                },
            };
            builder.Entity<Portfolio>().HasData(portfolios);
        }
    }
}