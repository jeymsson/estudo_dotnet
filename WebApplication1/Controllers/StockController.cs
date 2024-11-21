using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll() {
            var data = _context.Stock.ToList()
                        .Select(stock => stock.toStockDto());
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult getById([FromRoute] int id) {
            var stock = this._context.Stock.Find(id);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpPost]
        public IActionResult create([FromBody] CreateStockRequestDto stockDto) {
            var stockModel = stockDto.toCreateStockRequestDto();
            this._context.Stock.Add(stockModel);
            this._context.SaveChanges();
            return CreatedAtAction(nameof(getById), new { id = stockModel.Id }, stockModel.toStockDto());
        }

        [HttpPut("{id}")]
        public IActionResult update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto) {
            var stockModel = stockDto.toUpdateStockRequestDto();
            var stock = this._context.Stock.Find(id);
            if(stock == null) {
                return NotFound();
            }
            stock.Symbol = stockModel.Symbol;
            stock.CompanyName = stockModel.CompanyName;
            stock.Purchase = stockModel.Purchase;
            stock.LastDiv = stockModel.LastDiv;
            stock.Industry = stockModel.Industry;
            stock.MarketCap = stockModel.MarketCap;
            this._context.SaveChanges();
            return Ok(stock.toStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult delete([FromRoute] int id) {
            var stock = this._context.Stock.Find(id);
            if(stock == null) {
                return NotFound();
            }
            this._context.Stock.Remove(stock);
            this._context.SaveChanges();
            return NoContent();
        }
    }
}