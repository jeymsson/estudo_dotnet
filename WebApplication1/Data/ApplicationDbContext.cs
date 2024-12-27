using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Stock> Stock {get; set;}
        public DbSet<Comment> Comment {get; set;}
        public DbSet<AppUser> AppUser {get; set;}
        public DbSet<Portfolio> Portfolios {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>()
                .HasKey(p => new {p.AppUserId, p.StockId});
            builder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);
            builder.Entity<Portfolio>()
                .HasOne(p => p.Stock)
                .WithMany(s => s.Portfolios)
                .HasForeignKey(p => p.StockId);

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole{
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            List<AppUser> users = new List<AppUser>{
                new AppUser{
                    Id = "1",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@email.com",
                }
            };
            builder.Entity<AppUser>().HasData(users);

            List<Stock> stocks = new List<Stock> {
                new Stock{
                    Id = 1,
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Purchase = 100,
                    LastDiv = 0.82m,
                    Industry = "Technology",
                    MarketCap = 2000000000,
                },
                new Stock{
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

            List<Comment> comments = new List<Comment> {
                new Comment{
                    Id = 1,
                    StockId = 1,
                    Title = "Comment 1",
                    Content = "This is a comment",
                },
                new Comment{
                    Id = 2,
                    StockId = 1,
                    Title = "Comment 2",
                    Content = "This is another comment",
                }
            };
            builder.Entity<Comment>().HasData(comments);

            List<Portfolio> portfolios = new List<Portfolio> {
                new Portfolio{
                    AppUserId = "1",
                    StockId = 1,
                },
            };
            builder.Entity<Portfolio>().HasData(portfolios);
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<WebApplication1.Models.Stock>()
        //         .HasMany(s => s.Comments)
        //         .WithOne(c => c.Stock)
        //         .HasForeignKey(c => c.StockId);
        // }

    }
}