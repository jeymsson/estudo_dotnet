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
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) {
            this._context = context;
        }

        public async Task<List<Comment>> getAllAsync()
        {
            return await _context.Comment.ToListAsync();
        }

        public async Task<Comment?> AddAsync(Comment commentModel)
        {
            var stock = await this._context.Stock.FindAsync(commentModel.StockId);
            if(stock == null) {
                return null;
            }
            await this._context.AddAsync(commentModel);
            await this._context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var comment = await this.FindAsync(id);
            if(comment == null) {
                return null;
            }
            comment.Title = commentModel.Title;
            comment.Content = commentModel.Content;
            comment.StockId = commentModel.StockId;
            await this._context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> FindAsync(int id)
        {
            return await this._context.Comment.FindAsync(id);
        }

        public async Task<Comment> DeleteAsync(Comment commentModel)
        {
            var comment = this._context.Remove(commentModel);
            await this._context.SaveChangesAsync();
            return comment.Entity;
        }
    }
}