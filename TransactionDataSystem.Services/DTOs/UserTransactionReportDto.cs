using System;

namespace TransactionDataSystem.Services.DTOs
{
    public class UserTransactionReportDto
    {
        public Guid UserId { get; set; }
        public int TotalTransactions { get; set; }
    }
}