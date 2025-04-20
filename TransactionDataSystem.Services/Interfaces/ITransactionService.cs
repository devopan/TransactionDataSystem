using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionDataSystem.Services.DTOs;

namespace TransactionDataSystem.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionDto> GetTransactionByIdAsync(int id);
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createActionDto);
    }
}
