using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionDataSystem.Domain.Entities;
using TransactionDataSystem.Domain.Enums;
using TransactionDataSystem.Infrastructure.Data;
using TransactionDataSystem.Services.Enums;
using TransactionDataSystem.Services.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TransactionDataSystem.UnitTests.Services
{
    public class ReportingServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ReportingServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Seed the database
            using (var context = new ApplicationDbContext(_options))
            {
                SeedDatabase(context);
            }
        }

        private void SeedDatabase(ApplicationDbContext context)
        {
            // Add users
            var user1 = new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "UserOne", CreatedAt = DateTime.UtcNow.AddDays(-30) };
            var user2 = new User { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "UserTwo", CreatedAt = DateTime.UtcNow.AddDays(-20) };
            context.Users.AddRange(user1, user2);

            // Add actions
            var actions = new List<Domain.Entities.Transaction>
            {
                new Domain.Entities.Transaction { Id = 1, Amount = 100, TransactionType = TransactionTypeEnum.Debit, CreatedAt = DateTime.UtcNow.AddDays(-15) },
                new Domain.Entities.Transaction { Id = 2, Amount = 200, TransactionType = TransactionTypeEnum.Debit, CreatedAt = DateTime.UtcNow.AddDays(-14) },
                new Domain.Entities.Transaction { Id = 3, Amount = 300, TransactionType = TransactionTypeEnum.Debit, CreatedAt = DateTime.UtcNow.AddDays(-13) },
                new Domain.Entities.Transaction { Id = 4, Amount = 400, TransactionType = TransactionTypeEnum.Credit, CreatedAt = DateTime.UtcNow.AddDays(-12) },
                new Domain.Entities.Transaction { Id = 5, Amount = 500, TransactionType = TransactionTypeEnum.Credit, CreatedAt = DateTime.UtcNow.AddDays(-11) }
            };
            context.Transactions.AddRange(actions);

            // Add user actions
            var userTransactions = new List<UserTransaction>
            {
                new UserTransaction { Id = Guid.NewGuid(), UserId = user1.Id, TransactionId = 1 },
                new UserTransaction { Id = Guid.NewGuid(), UserId = user1.Id, TransactionId = 2 },
                new UserTransaction { Id = Guid.NewGuid(), UserId = user1.Id, TransactionId = 3 },
                new UserTransaction { Id = Guid.NewGuid(), UserId = user2.Id, TransactionId = 4 },
                new UserTransaction { Id = Guid.NewGuid(), UserId = user2.Id, TransactionId = 5 }
            };
            context.UserTransactions.AddRange(userTransactions);

            context.SaveChanges();
        }

        [Fact]
        public async Task GetHighVolumeTransactionsAsync_ByUser_ReturnsCorrectTransactionIds()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var service = new ReportingService(context);
            var from = DateTime.UtcNow.AddDays(-20);
            var to = DateTime.UtcNow;
            var limit = 2; // User1 has 3 actions, User2 has 2 actions

            // Act
            var result = await service.GetHighVolumeTransactionsAsync(from, to, limit, HighVolumeGroupingType.ByUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TransactionIds.Count); // Only User1's actions should be returned (3 actions)
            Assert.Contains(1, result.TransactionIds);
            Assert.Contains(2, result.TransactionIds);
            Assert.Contains(3, result.TransactionIds);
        }

        [Fact]
        public async Task GetHighVolumeTransactionsAsync_ByTransactionType_ReturnsCorrectTransactionIds()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var service = new ReportingService(context);
            var from = DateTime.UtcNow.AddDays(-20);
            var to = DateTime.UtcNow;
            var limit = 2; // Debit has 3 actions, Credit has 2 actions

            // Act
            var result = await service.GetHighVolumeTransactionsAsync(from, to, limit, HighVolumeGroupingType.ByTransactionType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TransactionIds.Count); // Only Debit type actions should be returned (3 actions)
            Assert.Contains(1, result.TransactionIds);
            Assert.Contains(2, result.TransactionIds);
            Assert.Contains(3, result.TransactionIds);
        }

        [Fact]
        public async Task GetHighVolumeTransactionsAsync_DateFiltering_ReturnsCorrectTransactionIds()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var service = new ReportingService(context);
            var from = DateTime.UtcNow.AddDays(-14); // Only include actions from day -14 and newer
            var to = DateTime.UtcNow.AddDays(-11);   // Only include actions up to day -11
            var limit = 0; // Include all actions in the date range

            // Act
            var result = await service.GetHighVolumeTransactionsAsync(from, to, limit, HighVolumeGroupingType.ByUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TransactionIds.Count); // Should return actions 3, 4, and 5
            Assert.Contains(3, result.TransactionIds);
            Assert.Contains(4, result.TransactionIds);
            Assert.Contains(5, result.TransactionIds);
        }
    }
}
