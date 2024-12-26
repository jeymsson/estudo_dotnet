using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dto.Portfolio
{
    public class CreatePortfolioRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Symbol must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "Symbol must be at most 100 characters")]
        public string Symbol { get; set; } = string.Empty;
    }
}