using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TransactionDataSystem.Services.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TransactionDataSystem.Web.Test.Integration.Controllers
{
    public class ReportingControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ReportingControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetTotalTransactionsByUser_ReturnsSuccessAndReport()
        {
            // Act
            var response = await _client.GetAsync("/api/reporting/users/transactions");

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<List<UserTransactionReportDto>>();
            Assert.NotNull(report);
            Assert.True(report.Count > 0);
        }

        [Fact]
        public async Task GetTotalTransactionsByType_ReturnsSuccessAndReport()
        {
            // Act
            var response = await _client.GetAsync("/api/reporting/transactions/types");

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<List<TransactionTypeReportDto>>();
            Assert.NotNull(report);
            Assert.True(report.Count > 0);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_WithValidParameters_ReturnsSuccessAndReport()
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fromDate = today.AddDays(-30).ToString("dd/MM/yyyy");
            var toDate = today.ToString("dd/MM/yyyy");

            // Act
            var response = await _client.GetAsync($"/api/reporting/high-volume-transactions?from={fromDate}&to={toDate}&limit=0&groupBy=user");

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<HighVolumeTransactionReportDto>();
            Assert.NotNull(report);
            Assert.NotNull(report.TransactionIds);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_WithInvalidDateFormat_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/reporting/high-volume-transactions?from=2023-01-01&to=2023-12-31&limit=5");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
