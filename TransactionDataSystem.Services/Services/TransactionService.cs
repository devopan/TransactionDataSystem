using System.Threading.Tasks;
using TransactionDataSystem.Domain.Entities;
using AutoMapper;
using TransactionDataSystem.Infrastructure.UnitOfWork;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Interfaces;

namespace TransactionDataSystem.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            var transactionIds = transactions.Select(x => x.Id);
            var userTransactions = await _unitOfWork.UserTransactions.FindAsync(x => transactionIds.Contains(x.TransactionId));
            var transactionsDtos = new List<TransactionDto>();
            foreach (var transaction in transactions)
            {
                var userTransaction = userTransactions.Where(x => x.TransactionId == transaction.Id).FirstOrDefault();
                if (userTransaction != null)
                {
                    var transactionDto = new TransactionDto();
                    transactionDto = _mapper.Map<TransactionDto>(transaction);
                    transactionDto.UserId = userTransaction.UserId;
                    transactionsDtos.Add(transactionDto);
                }
            }
            
            return transactionsDtos;
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            var userTransactions = await _unitOfWork.UserTransactions.FindAsync(x => x.TransactionId == transaction.Id);

            if (userTransactions.Any())
            {
                transactionDto.UserId = userTransactions.FirstOrDefault().UserId;
                return transactionDto;
            }

            throw new Exception($"UserId is missing from transaction with Id: {id}");
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            var transaction = _mapper.Map<Domain.Entities.Transaction>(createTransactionDto);
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();
            var userTransaction = new UserTransaction() { UserId = createTransactionDto.UserId, TransactionId = transaction.Id };
            await _unitOfWork.UserTransactions.AddAsync(userTransaction);
            await _unitOfWork.CompleteAsync();
            var actionDto = _mapper.Map<TransactionDto>(transaction);
            actionDto.UserId = userTransaction.UserId;
            return actionDto;
        }
    }
}