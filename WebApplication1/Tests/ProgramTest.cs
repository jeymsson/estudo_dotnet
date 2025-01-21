using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.interfaces;
using WebApplication1.Repository;
using Xunit;
using WebApplication1.Service;

namespace WebApplication1.Tests
{
    public class ProgramTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ProgramTest()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                });
            });
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetSecureEndpoint_UnauthorizedWithoutToken()
        {
            // Arrange
            var request = "/api/comment";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void ServicesAreRegistered()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("TestDatabase")
            );
            var inMemorySettings = new Dictionary<string, string?> {
                {"Jwt:SigningKey", "YourSigningKeyHere"},
                {"Jwt:Issuer", "YourIssuerHere"},
                {"Jwt:Audience", "YourAudienceHere"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();

            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            Assert.NotNull(serviceProvider.GetService<IStockRepository>());
            Assert.NotNull(serviceProvider.GetService<ICommentRepository>());
            Assert.NotNull(serviceProvider.GetService<IPortfolioRepository>());
            Assert.NotNull(serviceProvider.GetService<ITokenService>());
            Assert.NotNull(serviceProvider.GetService<ApplicationDbContext>());
        }
    }

}