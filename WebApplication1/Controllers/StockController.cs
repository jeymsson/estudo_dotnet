using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Custom.Attributes;
using WebApplication1.Data;
using WebApplication1.Dto.Stock;
using WebApplication1.Helpers;
using WebApplication1.interfaces;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [CustomAuthorize]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDbContext context, IStockRepository stockRepository)
        {
            this._context = context;
            this._stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var data = await this._stockRepository.getAllAsync(query);
            var dataDto = data.Select(stock => stock).ToList<Stock>();
            return Ok(dataDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var stock = await this._stockRepository.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateStockRequestDto stockDto) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var stockModel = stockDto.toCreateStockRequestDto();
            await this._stockRepository.AddAsync(stockModel);
            return CreatedAtAction(nameof(getById), new { id = stockModel.Id }, stockModel.toStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var stockModel = stockDto.toUpdateStockRequestDto();
            var stock = await this._stockRepository.UpdateAsync(id, stockModel);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> delete([FromRoute] int id) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var stock = await this._stockRepository.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            await this._stockRepository.DeleteAsync(stock);
            return NoContent();
        }
    }
}