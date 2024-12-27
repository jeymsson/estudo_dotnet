using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Dto.Stock;
using Xunit;

namespace WebApplication1.Tests.Dto.Stock
{
    public class UpdateStockRequestDtoTest
    {
        [Fact]
        public void UpdateStockRequestDto_ValidData_ShouldBeValid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AAPLX",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Tech",
                MarketCap = 1000
            };

            var validationResults = ValidateModel(dto);

            Assert.Empty(validationResults);
        }

        [Fact]
        public void UpdateStockRequestDto_InvalidSymbol_ShouldBeInvalid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AP",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Tech",
                MarketCap = 2000
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("Symbol"));
        }

        [Fact]
        public void UpdateStockRequestDto_InvalidCompanyName_ShouldBeInvalid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Ap",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Tech",
                MarketCap = 2000
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("CompanyName"));
        }

        [Fact]
        public void UpdateStockRequestDto_InvalidPurchase_ShouldBeInvalid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 0.50m,
                LastDiv = 0.82m,
                Industry = "Tech",
                MarketCap = 2000
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("Purchase"));
        }

        [Fact]
        public void UpdateStockRequestDto_InvalidLastDiv_ShouldBeInvalid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.0001m,
                Industry = "Tech",
                MarketCap = 2000
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("LastDiv"));
        }

        [Fact]
        public void UpdateStockRequestDto_InvalidIndustry_ShouldBeInvalid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AAPLX",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                MarketCap = 1000
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("Industry"));
        }

        [Fact]
        public void UpdateStockRequestDto_InvalidMarketCap_ShouldBeInvalid()
        {
            var dto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Tech",
                MarketCap = 10000
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("MarketCap"));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}