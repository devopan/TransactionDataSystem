using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Interfaces;

namespace TransactionDataSystem.Web.Controllers
{
    public class TransactionsController : BaseController<TransactionDto, CreateTransactionDto, int>
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService actionService)
        {
            _transactionService = actionService;
        }

        protected override async Task<TransactionDto> ReadSingleAsync(int id)
        {
            return await _transactionService.GetTransactionByIdAsync(id);
        }

        protected override async Task<TransactionDto> CreateAsync(CreateTransactionDto createDto)
        {
            return await _transactionService.CreateTransactionAsync(createDto);
        }

        protected override int GetEntityId(TransactionDto entity)
        {
            return entity.Id;
        }

        protected override async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            return await _transactionService.GetAllTransactionsAsync();
        }
    }
}