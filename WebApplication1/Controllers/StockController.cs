using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto.Stock;
using WebApplication1.interfaces;
using WebApplication1.Mappers;

namespace WebApplication1.Controllers
{
    [Route("api/stock")]
    [ApiController]
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
        public async  Task<IActionResult> GetAll() {
            var data = await this._stockRepository.getAllAsync();
            var dataDto = data.Select(stock => stock.toStockDto());
            return Ok(dataDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            var stock = await this._stockRepository.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateStockRequestDto stockDto) {
            var stockModel = stockDto.toCreateStockRequestDto();
            await this._stockRepository.AddAsync(stockModel);
            return CreatedAtAction(nameof(getById), new { id = stockModel.Id }, stockModel.toStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto) {
            var stockModel = stockDto.toUpdateStockRequestDto();
            var stock = await this._stockRepository.UpdateAsync(id, stockModel);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> delete([FromRoute] int id) {
            var stock = await this._stockRepository.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            await this._stockRepository.DeleteAsync(stock);
            return NoContent();
        }
    }
}