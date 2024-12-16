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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
        }


        // public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //     : base(options)
        // {
        // }

        // public DbSet<WebApplication1.Models.Stock> Stocks { get; set; } = default!;
        // public DbSet<WebApplication1.Models.Comment> Comments { get; set; } = default!;

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<WebApplication1.Models.Stock>()
        //         .HasMany(s => s.Comments)
        //         .WithOne(c => c.Stock)
        //         .HasForeignKey(c => c.StockId);
        // }

    }
}