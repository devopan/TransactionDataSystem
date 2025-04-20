using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Enums;

namespace TransactionDataSystem.Services.Interfaces
{
    public interface IReportingService
    {
        Task<IEnumerable<UserTransactionReportDto>> GetTotalTransactionsByUserAsync();
        Task<IEnumerable<TransactionTypeReportDto>> GetTotalTransactionsByTypeAsync();
        Task<HighVolumeTransactionReportDto> GetHighVolumeTransactionsAsync(DateTime from, DateTime to, int limit, HighVolumeGroupingType groupingType);
    }
}
