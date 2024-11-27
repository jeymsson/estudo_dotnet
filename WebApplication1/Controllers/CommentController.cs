using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Dto.Comment;
using WebApplication1.interfaces;
using WebApplication1.Mappers;

namespace WebApplication1.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentRepository _repository;
        public CommentController(ApplicationDbContext context, ICommentRepository repository)
        {
            this._context = context;
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> getAll() {
            var data = await this._repository.getAllAsync();
            var dataDto = data.Select(comment => comment.toCommentDto());
            return Ok(dataDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            var data = await this._repository.FindAsync(id);
            if(data == null) {
                return NotFound();
            }
            return Ok(data.toCommentDto());
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateCommentRequestDto request) {
            var data = request.toCreateCommentRequestDto();
            await this._repository.AddAsync(data);
            return Ok(data.toCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromBody] UpdateCommentRequestDto request, [FromRoute] int id) {
            var data = await this._repository.FindAsync(id);
            if(data == null) {
                return NotFound();
            }
            var dataDto = request.toUpdateCommentRequestDto();
            await this._repository.UpdateAsync(id, dataDto);
            return Ok(dataDto.toCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> delete([FromRoute] int id) {
            var data = await this._repository.FindAsync(id);
            if(data == null) {
                return NotFound();
            }
            await this._repository.DeleteAsync(data);
            return Ok(data.toCommentDto());
        }

    }
}