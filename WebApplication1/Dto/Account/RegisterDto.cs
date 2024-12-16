using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dto.Account
{
    public class RegisterDto
    {
        [Required]
        public String? Username { get; set; }
        [Required]
        [EmailAddress]
        public String? Email { get; set; }
        [Required]
        public String? Password { get; set; }
    }
}