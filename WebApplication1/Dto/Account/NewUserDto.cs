using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dto.Account
{
    public class NewUserDto
    {
        public String Username { get; set; }
        public String Email { get; set; }
        public String Token { get; set; }
    }
}