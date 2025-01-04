using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Custom.Attributes;
using WebApplication1.Data;
using WebApplication1.Dto.Comment;
using WebApplication1.interfaces;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [CustomAuthorize]
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
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var data = await this._repository.getAllAsync();
            var dataDto = data.ToList<Comment>();
            return Ok(dataDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var data = await this._repository.FindAsync(id);
            if(data == null) {
                return NotFound();
            }
            return Ok(data.toCommentDto());
        }

        [HttpPost]
        public async Task<IActionResult> create([FromRoute] int stock_id, [FromBody] CreateCommentRequestDto commentDto) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(!await this._stockRepository.stockExists(stock_id)) {
                return BadRequest("Stock not exists");
            }
            var data = commentDto.toCommentFromCreate(stock_id);
            var model = await this._repository.AddAsync(data);
            return Ok(model);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> update([FromRoute] int stock_id, [FromBody] UpdateCommentRequestDto request) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(!await this._stockRepository.stockExists(stock_id)) {
                return BadRequest("Stock not exists");
            }
            var dataDto = request.toCommentFromUpdate(stock_id);
            await this._repository.UpdateAsync(stock_id, dataDto);
            return Ok(dataDto.toCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> delete([FromRoute] int id) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var data = await this._repository.FindAsync(id);
            if(data == null) {
                return NotFound();
            }
            await this._repository.DeleteAsync(data);
            return Ok(data.toCommentDto());
        }
    }
}