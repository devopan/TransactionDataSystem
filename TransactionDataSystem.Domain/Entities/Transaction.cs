using System;
using TransactionDataSystem.Domain.Enums;

namespace TransactionDataSystem.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}