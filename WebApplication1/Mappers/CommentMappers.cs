using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Dto.Comment;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto toCommentDto(this Comment commentModel) {
            return new CommentDto {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = commentModel.StockId
            };
        }

        public static Comment toCommentFromCreate(this CreateCommentRequestDto commentModel, int stock_id) {
            return new Comment {
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = stock_id,
            };
        }

        public static Comment toCommentFromUpdate(this UpdateCommentRequestDto commentModel, int stock_id) {
            return new Comment {
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = stock_id
            };
        }
    }
}