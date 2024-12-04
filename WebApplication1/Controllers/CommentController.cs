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
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentRepository _repository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ApplicationDbContext context, ICommentRepository repository, IStockRepository stockRepository)
        {
            this._context = context;
            this._repository = repository;
            this._stockRepository = stockRepository;
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
        public async Task<IActionResult> create([FromRoute] int stock_id, [FromBody] CreateCommentRequestDto commentDto) {
            if(!await this._stockRepository.stockExists(stock_id)) {
                return BadRequest("Stock not exists");
            }
            var data = commentDto.toCommentFromCreate(stock_id);
            var model = await this._repository.AddAsync(data);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromRoute] int stock_id, [FromBody] UpdateCommentRequestDto request) {
            if(!await this._stockRepository.stockExists(stock_id)) {
                return BadRequest("Stock not exists");
            }
            var dataDto = request.toCommentFromUpdate(stock_id);
            await this._repository.UpdateAsync(stock_id, dataDto);
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