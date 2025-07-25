
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_Collection.Core.Interfaces
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync();
        Task<List<ExpenseListResponse>> GetExpenseListAsync(ExpenseFilterRequest filter, Guid userId);
        Task<List<ExpenseSummaryResponse>> GetExpenseSummaryAsync(ExpenseFilterRequest filter, Guid userId);
        Task<List<ExpenseReportResponse>> GetExpenseReportAsync(ExpenseFilterRequest filter, Guid userId);
        Task<ExpenseDetailsResponse> GetExpenseDetailsAsync(Guid expenseId, Guid userId);
        Task<ExpenseSuggestionList> GetExpenseSuggestionListAsync(Guid userId);
        Task<Guid> AddAsync(ExpenseRequest expense, Guid userId);
        Task<bool> UpdateAsync(ExpenseRequest expense, Guid userId);
        Task<bool> DeleteAsync(Guid expenseId, Guid userId);
    }
}
