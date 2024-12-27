using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Custom.Attributes
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Custom authorization logic here
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult("You are unauthorized yet")
                {
                    StatusCode = (int) HttpStatusCode.Unauthorized,
                };
            }
        }
    }
}