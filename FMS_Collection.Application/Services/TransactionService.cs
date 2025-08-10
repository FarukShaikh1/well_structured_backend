using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System.Data;

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
        public Task<List<TransactionSuggestionList>> GetTransactionSuggestionListAsync(Guid userId) => _repository.GetTransactionSuggestionListAsync(userId);
        public Task<bool> DeleteTransactionAsync(Guid TransactionId, Guid userId) => _repository.DeleteAsync(TransactionId, userId);

        public async Task<Guid> AddTransactionAsync(TransactionRequest transaction, Guid userId)
        {
            var (splitTable, hasValid) = BuildSplitTable(transaction);
            return hasValid
                ? await _repository.AddAsync(transaction, splitTable, userId)
                : Guid.Empty;
        }

        public async Task<bool> UpdateTransactionAsync(TransactionRequest transaction, Guid userId)
        {
            var (splitTable, hasValid) = BuildSplitTable(transaction);
            return hasValid && await _repository.UpdateAsync(transaction, splitTable, userId);
        }

        private (DataTable SplitTable, bool HasValidRecords) BuildSplitTable(TransactionRequest transaction)
        {
            var splitTable = new DataTable();
            splitTable.Columns.Add("AccountId", typeof(Guid));
            splitTable.Columns.Add("Amount", typeof(decimal));
            splitTable.Columns.Add("Category", typeof(string));

            bool validRecordExists = false;

            foreach (var split in transaction.AccountSplits)
            {
                if (split.Amount != 0 && split.Category.HasValue)
                {
                    validRecordExists = true;
                }

                splitTable.Rows.Add(
                    split.AccountId,
                    split.Amount,
                    split.Category?.ToString() ?? string.Empty
                );
            }

            return (splitTable, validRecordExists);
        }
    }
}
