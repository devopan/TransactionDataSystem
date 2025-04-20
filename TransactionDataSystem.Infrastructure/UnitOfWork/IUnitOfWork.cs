using System;
using System.Threading.Tasks;
using TransactionDataSystem.Infrastructure.Repositories;
using TransactionDataSystem.Domain.Entities;

namespace TransactionDataSystem.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Domain.Entities.Transaction> Transactions { get; }
        IRepository<UserTransaction> UserTransactions { get; }
        Task<int> CompleteAsync();
    }
}