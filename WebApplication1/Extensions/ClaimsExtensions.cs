using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? getUsername(this ClaimsPrincipal user) {
            return user.Claims.SingleOrDefault(x => {
                return x.Type.Equals(ClaimTypes.GivenName);
            })?.Value;
        }
    }
}