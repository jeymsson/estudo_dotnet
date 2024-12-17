using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dto.Account
{
    public class LoginDto
    {
        [Required]
        public String? Username { get; set; }
        [Required]
        public String? Password { get; set; }
    }
}