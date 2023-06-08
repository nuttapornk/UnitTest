using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace YourProject.IntegrationTests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<YourProject.Startup>>
    {
        private readonly WebApplicationFactory<YourProject.Startup> _factory;

        public ApiIntegrationTests(WebApplicationFactory<YourProject.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllItems_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/items");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetItemById_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            int itemId = 1;

            // Act
            var response = await client.GetAsync($"/api/items/{itemId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetItemById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var client = _factory.CreateClient();
            int itemId = 999;

            // Act
            var response = await client.GetAsync($"/api/items/{itemId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Add more tests for other API endpoints as needed
    }
}
