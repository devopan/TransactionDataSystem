using System;

namespace TransactionDataSystem.Services.DTOs
{
    public class UserTransactionDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int TransactionId { get; set; }
    }
}