using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dto.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Symbol must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "Symbol must be at most 100 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "CompanyName must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "CompanyName must be at most 100 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry must be at most 10 characters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000)]
        public long MarketCap { get; set; }
    }
}