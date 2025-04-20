using System;
using System.Transactions;

namespace TransactionDataSystem.Domain.Entities
{
    public class UserTransaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TransactionId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}