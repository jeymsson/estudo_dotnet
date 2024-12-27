using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data.Seeds
{
    public class AppUserSeed
    {
        public AppUserSeed(ModelBuilder builder)
        {
            List<AppUser> users = new List<AppUser>{
                new AppUser{
                    Id = "1",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@email.com",
                }
            };
            builder.Entity<AppUser>().HasData(users);
        }
    }
}