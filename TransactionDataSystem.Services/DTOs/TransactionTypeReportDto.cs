using TransactionDataSystem.Domain.Enums;

namespace TransactionDataSystem.Services.DTOs
{
    public class TransactionTypeReportDto
    {
        public TransactionTypeEnum TransactionType { get; set; }
        public int TotalTransactions { get; set; }
    }
}