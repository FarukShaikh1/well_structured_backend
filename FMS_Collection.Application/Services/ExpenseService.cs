using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class ExpenseService
    {
        private readonly IExpenseRepository _repository;
        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Expense>> GetAllExpensesAsync() => _repository.GetAllAsync();
        public Task<List<ExpenseListResponse>> GetExpenseListAsync(ExpenseFilterRequest filter, Guid userId) => _repository.GetExpenseListAsync(filter, userId);
        public Task<List<ExpenseSummaryResponse>> GetExpenseSummaryAsync(ExpenseFilterRequest filter, Guid userId) => _repository.GetExpenseSummaryAsync(filter, userId);
        public Task<List<ExpenseReportResponse>> GetExpenseReportAsync(ExpenseFilterRequest filter, Guid userId) => _repository.GetExpenseReportAsync(filter, userId);
        public Task<ExpenseDetailsResponse> GetExpenseDetailsAsync(Guid expenseId, Guid userId) => _repository.GetExpenseDetailsAsync(expenseId, userId);
        public Task<ExpenseSuggestionList> GetExpenseSuggestionListAsync(Guid userId) => _repository.GetExpenseSuggestionListAsync(userId);
        public Task<Guid> AddExpenseAsync(ExpenseRequest Expense,Guid userId) => _repository.AddAsync(Expense, userId);
        public Task<bool> UpdateExpenseAsync(ExpenseRequest Expense, Guid userId) => _repository.UpdateAsync(Expense, userId);
        public Task<bool> DeleteExpenseAsync(Guid expenseId, Guid userId) => _repository.DeleteAsync(expenseId, userId);
    }
}
