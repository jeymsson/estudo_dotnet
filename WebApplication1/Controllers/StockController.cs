using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
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
    }
}