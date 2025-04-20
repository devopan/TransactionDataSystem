using TransactionDataSystem.Domain.Entities;
using TransactionDataSystem.Infrastructure.Data;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Enums;
using TransactionDataSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionDataSystem.Infrastructure.Data;
using TransactionDataSystem.Services.Interfaces;

namespace TransactionDataSystem.Services.Services
{
    public class ReportingService : IReportingService
    {
        private readonly ApplicationDbContext _context;

        public ReportingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTransactionReportDto>> GetTotalTransactionsByUserAsync()
        {
            var report = await _context.UserTransactions
                .GroupBy(ua => ua.UserId)
                .Select(g => new UserTransactionReportDto
                {
                    UserId = g.Key,
                    TotalTransactions = g.Count()
                })
                .ToListAsync();

            return report;
        }

        public async Task<IEnumerable<TransactionTypeReportDto>> GetTotalTransactionsByTypeAsync()
        {
            var report = await _context.Transactions
                .GroupBy(a => a.TransactionType)
                .Select(g => new TransactionTypeReportDto
                {
                    TransactionType = g.Key,
                    TotalTransactions = g.Count()
                })
                .ToListAsync();

            return report;
        }

        public async Task<HighVolumeTransactionReportDto> GetHighVolumeTransactionsAsync(DateTime from, DateTime to, int limit, HighVolumeGroupingType groupingType)
        {
            // Ensure dates are in UTC for consistent comparison
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            // Filter actions by date range
            var transactionsInRange = _context.Transactions
                .Where(a => a.CreatedAt >= from && a.CreatedAt <= to);

            var result = new HighVolumeTransactionReportDto();

            if (groupingType == HighVolumeGroupingType.ByUser)
            {
                // Group by user and count actions
                var userTransactionCounts = _context.UserTransactions
                    .Where(ua => transactionsInRange.Any(a => a.Id == ua.TransactionId))
                    .GroupBy(ua => ua.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TransactionIds = g.Select(ua => ua.TransactionId).ToList(),
                        Count = g.Count()
                    })
                    .ToList()
                    .Where(x => x.Count > limit);

                // Collect all transaction IDs from users who have more than the limit
                foreach (var userTransaction in userTransactionCounts)
                {
                    result.TransactionIds.AddRange(userTransaction.TransactionIds);
                }
            }
            else // ByTransactionType
            {
                // Group by transaction type and count

                var actionTypeCounts = transactionsInRange
                    .GroupBy(a => a.TransactionType)
                    .Select(g => new
                    {
                        TransactionType = g.Key,
                        TransactionIds = g.Select(a => a.Id).ToList(),
                        Count = g.Count()
                    })
                    .ToList()
                    .Where(x => x.Count > limit);

                // Collect all transaction IDs from transaction types that have more than the limit
                foreach (var actionType in actionTypeCounts)
                {
                    result.TransactionIds.AddRange(actionType.TransactionIds);
                }
            }

            // Remove duplicates and return
            result.TransactionIds = result.TransactionIds.Distinct().ToList();
            return result;
        }
    }
}
