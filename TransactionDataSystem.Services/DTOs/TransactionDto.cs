using System;
using TransactionDataSystem.Domain.Enums;

namespace TransactionDataSystem.Services.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}