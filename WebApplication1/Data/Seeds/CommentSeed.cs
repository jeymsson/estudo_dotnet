using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data.Seeds
{
    public class CommentSeed
    {
        public CommentSeed(ModelBuilder builder)
        {
            List<Comment> comments = new List<Comment> {
                new Comment {
                    Id = 1,
                    StockId = 1,
                    Title = "Comment 1",
                    Content = "This is a comment",
                },
                new Comment {
                    Id = 2,
                    StockId = 1,
                    Title = "Comment 2",
                    Content = "This is another comment",
                }
            };
            builder.Entity<Comment>().HasData(comments);
        }
    }
}