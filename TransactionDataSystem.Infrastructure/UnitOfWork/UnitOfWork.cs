using System;
using System.Threading.Tasks;
using TransactionDataSystem.Infrastructure.Data;
using TransactionDataSystem.Infrastructure.Repositories;
using TransactionDataSystem.Domain.Entities;

namespace TransactionDataSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<User> _userRepository;
        private IRepository<Domain.Entities.Transaction> _actionRepository;
        private IRepository<UserTransaction> _userTransactionRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _userRepository ??= new Repository<User>(_context);

        public IRepository<Domain.Entities.Transaction> Transactions => _actionRepository ??= new Repository<Domain.Entities.Transaction>(_context);

        public IRepository<UserTransaction> UserTransactions => _userTransactionRepository ??= new Repository<UserTransaction>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}