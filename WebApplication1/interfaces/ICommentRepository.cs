using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> getAllAsync();
        Task<Comment?> FindAsync(int id);
        Task<Comment?> AddAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int id, Comment commentModel);
        Task<Comment> DeleteAsync(Comment commentModel);

    }
}