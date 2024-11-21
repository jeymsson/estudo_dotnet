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

        public static Comment toCreateCommentRequestDto(this CreateCommentRequestDto commentModel) {
            return new Comment {
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = commentModel.StockId
            };
        }

        public static Comment toUpdateCommentRequestDto(this UpdateCommentRequestDto commentModel) {
            return new Comment {
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = commentModel.StockId
            };
        }
    }
}