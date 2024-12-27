using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data.Seeds
{
    public class BuildSeeds
    {
        protected ModelBuilder builder;
        public BuildSeeds(ModelBuilder builder)
        {
            this.builder = builder;

            new RoleSeed(builder);
            new AppUserSeed(builder);
            new StockSeed(builder);
            new CommentSeed(builder);
            new PortfolioSeed(builder);
        }
    }
}