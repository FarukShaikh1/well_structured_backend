using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _repository;
        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Transaction>> GetAllTransactionsAsync() => _repository.GetAllAsync();
        public Task<List<TransactionListResponse>> GetTransactionListAsync(TransactionFilterRequest filter, Guid userId) => _repository.GetTransactionListAsync(filter, userId);
        public Task<List<TransactionSummaryResponse>> GetTransactionSummaryAsync(TransactionFilterRequest filter, Guid userId) => _repository.GetTransactionSummaryAsync(filter, userId);
        public Task<List<TransactionReportResponse>> GetTransactionReportAsync(TransactionFilterRequest filter, Guid userId) => _repository.GetTransactionReportAsync(filter, userId);
        public Task<TransactionDetailsResponse> GetTransactionDetailsAsync(Guid TransactionId, Guid userId) => _repository.GetTransactionDetailsAsync(TransactionId, userId);
        public Task<TransactionSuggestionList> GetTransactionSuggestionListAsync(Guid userId) => _repository.GetTransactionSuggestionListAsync(userId);
        public Task<Guid> AddTransactionAsync(TransactionRequest Transaction,Guid userId) => _repository.AddAsync(Transaction, userId);
        public Task<bool> UpdateTransactionAsync(TransactionRequest Transaction, Guid userId) => _repository.UpdateAsync(Transaction, userId);
        public Task<bool> DeleteTransactionAsync(Guid TransactionId, Guid userId) => _repository.DeleteAsync(TransactionId, userId);
    }
}
