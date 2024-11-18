using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

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
            return Ok(_context.Stock.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult getById([FromRoute] int id) {
            var stock = this._context.Stock.Find(id);
            if(stock == null) {
                return NotFound();
            }
            return Ok(stock);
        }
    }
}