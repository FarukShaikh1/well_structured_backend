using FMS_Collection.Core.Common;
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

        public async Task<ServiceResponse<List<Transaction>>> GetAllTransactionsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionsFetchedSuccessfully
            );
        }
        public async Task<ServiceResponse<List<TransactionListResponse>>> GetTransactionListAsync(TransactionFilterRequest filter, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetTransactionListAsync(filter, userId),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionListFetchedSuccessfully
            );
        }
        public async Task<ServiceResponse<List<TransactionSummaryResponse>>> GetTransactionSummaryAsync(TransactionFilterRequest filter, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetTransactionSummaryAsync(filter, userId),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionSummaryFetchedSuccessfully
            );
        }
        public async Task<ServiceResponse<List<TransactionSummaryResponse>>> GetBalanceSummaryAsync(TransactionFilterRequest filter, Guid userId)
        {
            
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetBalanceSummaryAsync(filter, userId),
                FMS_Collection.Core.Constants.Constants.Messages.BalanceSummaryFetchedSuccessfully
            );
        }
        public async Task<ServiceResponse<List<TransactionReportResponse>>> GetTransactionReportAsync(TransactionFilterRequest filter, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetTransactionReportAsync(filter, userId),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionReportFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<TransactionDetailsResponse>> GetTransactionDetailsAsync(Guid TransactionId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetTransactionDetailsAsync(TransactionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionDetailsFetchedSuccessfully
            );
        }
        public async Task<ServiceResponse<List<TransactionSuggestionList>>> GetTransactionSuggestionListAsync(Guid userId)
        {

            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetTransactionSuggestionListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionSuggestionsFetchedSuccessfully
            );
        }
        public async Task<ServiceResponse<bool>> DeleteTransactionAsync(Guid TransactionId, Guid userId)
        {

            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(TransactionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.TransactionDeletedSuccessfully
            );
        }
        public async Task<ServiceResponse<Guid>> AddTransactionAsync(TransactionRequest transaction, Guid userId)
        {
            var response = new ServiceResponse<Guid>();
            var (splitTable, hasValid) = BuildSplitTable(transaction);
            if (hasValid)
            {
                return await ServiceExecutor.ExecuteAsync(
                    () => _repository.AddAsync(transaction, splitTable, userId),
                    FMS_Collection.Core.Constants.Constants.Messages.TransactionCreatedSuccessfully
                );
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateTransactionAsync(TransactionRequest transaction, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            var (splitTable, hasValid) = BuildSplitTable(transaction);
            if (hasValid)
            { //&& await _repository.UpdateAsync(transaction, splitTable, userId);
                return await ServiceExecutor.ExecuteAsync(
                    () => _repository.UpdateAsync(transaction, splitTable, userId),
                    FMS_Collection.Core.Constants.Constants.Messages.TransactionUpdatedSuccessfully
                );
            }
            return response;
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
