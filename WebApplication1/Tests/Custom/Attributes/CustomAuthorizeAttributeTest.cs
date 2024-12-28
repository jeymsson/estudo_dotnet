using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using WebApplication1.Custom.Attributes;
using Xunit;

namespace WebApplication1.Tests.Custom.Attributes
{
    public class CustomAuthorizeAttributeTest
    {
        [Fact]
        public void OnAuthorization_UserIsNotAuthenticated_ReturnsUnauthorizedResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var context = new AuthorizationFilterContext(
                new ActionContext(
                    httpContext,
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                ),
                new List<IFilterMetadata>()
            );

            var customAuthorizeAttribute = new CustomAuthorizeAttribute();

            // Act
            customAuthorizeAttribute.OnAuthorization(context);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.Unauthorized, jsonResult.StatusCode);
            Assert.Equal("You are unauthorized yet", jsonResult.Value);
        }

        [Fact]
        public void OnAuthorization_UserIsAuthenticated_DoesNotSetResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var userMock = Mock.Of<System.Security.Principal.IIdentity>(
                u => u.IsAuthenticated == true &&
                u.AuthenticationType == "test"
            );
            httpContext.User = new System.Security.Claims.ClaimsPrincipal(userMock);

            var context = new AuthorizationFilterContext(
                new ActionContext(
                    httpContext,
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                ),
                new List<IFilterMetadata>()
            );

            var customAuthorizeAttribute = new CustomAuthorizeAttribute();

            // Act
            customAuthorizeAttribute.OnAuthorization(context);

            // Assert context result is null
            Assert.Null(context.Result);
        }
    }
}