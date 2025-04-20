using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TransactionDataSystem.Domain.Enums;
using TransactionDataSystem.Services.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TransactionDataSystem.Web.Test.Integration.Controllers
{
    public class TransactionsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public TransactionsControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsSuccessAndTransaction()
        {
            // Arrange
            var validId = 1;

            // Act
            var response = await _client.GetAsync($"/api/transactions/{validId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var transaction = await response.Content.ReadFromJsonAsync<TransactionDto>();
            Assert.NotNull(transaction);
            Assert.Equal(validId, transaction.Id);
            Assert.Equal(100.50m, transaction.Amount);
            Assert.Equal(TransactionTypeEnum.Debit, transaction.TransactionType);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var response = await _client.GetAsync($"/api/actions/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsSuccessAndCreatedTransaction()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                Amount = 75.25m,
                TransactionType = TransactionTypeEnum.Debit
            };
            var content = new StringContent(
                JsonSerializer.Serialize(createTransactionDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/transactions", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var transaction = await response.Content.ReadFromJsonAsync<TransactionDto>();
            Assert.NotNull(transaction);
            Assert.True(transaction.Id > 0);
            Assert.Equal(75.25m, transaction.Amount);
            Assert.Equal(TransactionTypeEnum.Debit, transaction.TransactionType);
        }
    }
}
