using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto.Stock;
using WebApplication1.Mappers;

namespace WebApplication1.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async  Task<IActionResult> GetAll() {
            var data = await _context.Stock.ToListAsync();
            var dataDto = data.Select(stock => stock.toStockDto());
            return Ok(dataDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            var stock = await this._context.Stock.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateStockRequestDto stockDto) {
            var stockModel = stockDto.toCreateStockRequestDto();
            await this._context.Stock.AddAsync(stockModel);
            await this._context.SaveChangesAsync();
            return CreatedAtAction(nameof(getById), new { id = stockModel.Id }, stockModel.toStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto) {
            var stockModel = stockDto.toUpdateStockRequestDto();
            var stock = await this._context.Stock.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            stock.Symbol = stockModel.Symbol;
            stock.CompanyName = stockModel.CompanyName;
            stock.Purchase = stockModel.Purchase;
            stock.LastDiv = stockModel.LastDiv;
            stock.Industry = stockModel.Industry;
            stock.MarketCap = stockModel.MarketCap;
            await this._context.SaveChangesAsync();
            return Ok(stock.toStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> delete([FromRoute] int id) {
            var stock = await this._context.Stock.FindAsync(id);
            if(stock == null) {
                return NotFound();
            }
            this._context.Stock.Remove(stock);
            await this._context.SaveChangesAsync();
            return NoContent();
        }
    }
}