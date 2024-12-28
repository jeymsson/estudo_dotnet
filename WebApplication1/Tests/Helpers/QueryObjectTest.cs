using System;
using Xunit;
using WebApplication1.Helpers;

namespace WebApplication1.Tests.Helpers
{
    public class QueryObjectTest
    {
        [Fact]
        public void SortBy_DefaultValue_ShouldBeEmptyString()
        {
            // Arrange
            var queryObject = new QueryObject();

            // Act
            var sortBy = queryObject.SortBy;

            // Assert
            Assert.Equal(string.Empty, sortBy);
        }

        [Fact]
        public void SortBy_SetValue_ShouldReturnSetValue()
        {
            // Arrange
            var queryObject = new QueryObject();
            var expectedValue = "CompanyName";

            // Act
            queryObject.SortBy = expectedValue;
            var sortBy = queryObject.SortBy;

            // Assert
            Assert.Equal(expectedValue, sortBy);
        }
    }
}