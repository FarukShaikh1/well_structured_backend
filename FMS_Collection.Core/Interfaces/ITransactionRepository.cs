
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_Collection.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<List<TransactionListResponse>> GetTransactionListAsync(TransactionFilterRequest filter, Guid userId);
        Task<List<TransactionSummaryResponse>> GetTransactionSummaryAsync(TransactionFilterRequest filter, Guid userId);
        Task<List<TransactionReportResponse>> GetTransactionReportAsync(TransactionFilterRequest filter, Guid userId);
        Task<TransactionDetailsResponse> GetTransactionDetailsAsync(Guid TransactionId, Guid userId);
        Task<List<TransactionSuggestionList>> GetTransactionSuggestionListAsync(Guid userId);
        Task<Guid> AddAsync(TransactionRequest Transaction, DataTable splitTable, Guid userId);
        Task<bool> UpdateAsync(TransactionRequest Transaction, DataTable splitTable, Guid userId);
        Task<bool> DeleteAsync(Guid TransactionId, Guid userId);
    }
}
