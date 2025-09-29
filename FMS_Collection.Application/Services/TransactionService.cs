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
            var response = new ServiceResponse<List<Transaction>>();
            try
            {
                var data = await _repository.GetAllAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<List<TransactionListResponse>>> GetTransactionListAsync(TransactionFilterRequest filter, Guid userId)
        {
            var response = new ServiceResponse<List<TransactionListResponse>>();
            try
            {
                var data = await _repository.GetTransactionListAsync(filter, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionListFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<List<TransactionSummaryResponse>>> GetTransactionSummaryAsync(TransactionFilterRequest filter, Guid userId)
        {
            var response = new ServiceResponse<List<TransactionSummaryResponse>>();
            try
            {
                var data = await _repository.GetTransactionSummaryAsync(filter, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionSummaryFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<List<TransactionSummaryResponse>>> GetBalanceSummaryAsync(TransactionFilterRequest filter, Guid userId)
        {
            
            var response = new ServiceResponse<List<TransactionSummaryResponse>>();
            try
            {
                var data = await _repository.GetBalanceSummaryAsync(filter, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.BalanceSummaryFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<List<TransactionReportResponse>>> GetTransactionReportAsync(TransactionFilterRequest filter, Guid userId)
        {
            var response = new ServiceResponse<List<TransactionReportResponse>>();
            try
            {
                var data = await _repository.GetTransactionReportAsync(filter, userId);

                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionReportFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<TransactionDetailsResponse>> GetTransactionDetailsAsync(Guid TransactionId, Guid userId)
        {
            var response = new ServiceResponse<TransactionDetailsResponse>();
            try
            {
                var data = await _repository.GetTransactionDetailsAsync(TransactionId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionDetailsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<List<TransactionSuggestionList>>> GetTransactionSuggestionListAsync(Guid userId)
        {

            var response = new ServiceResponse<List<TransactionSuggestionList>>();
            try
            {
                var data = await _repository.GetTransactionSuggestionListAsync(userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionSuggestionsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteTransactionAsync(Guid TransactionId, Guid userId)
        {

            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteAsync(TransactionId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionDeletedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<Guid>> AddTransactionAsync(TransactionRequest transaction, Guid userId)
        {
            var response = new ServiceResponse<Guid>();
            var (splitTable, hasValid) = BuildSplitTable(transaction);
            if (hasValid)
            {
                try
                {
                    var data = await _repository.AddAsync(transaction, splitTable, userId);
                    response.Success = true;
                    response.Data = data;
                    response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionCreatedSuccessfully;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Data = Guid.Empty;
                    response.Message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateTransactionAsync(TransactionRequest transaction, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            var (splitTable, hasValid) = BuildSplitTable(transaction);
            if (hasValid)
            { //&& await _repository.UpdateAsync(transaction, splitTable, userId);
                try
                {
                    var data = await _repository.UpdateAsync(transaction, splitTable, userId);
                    response.Success = true;
                    response.Data = data;
                    response.Message = FMS_Collection.Core.Constants.Constants.Messages.TransactionUpdatedSuccessfully;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = ex.Message;
                }
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
