namespace FMS_Collection.Core.Response
{
    public class BudgetWiseTransactionReportResponse
    {
        public string? CategoryName { get; set; }
        public decimal? BudgetAmount { get; set; }
        public decimal? TotalExpense { get; set; }
        public decimal? RemainingBudget { get; set; }
        public bool? IsOverSpent { get; set; }
        public int? BudgetMonths { get; set; }

        public DateOnly? FirstDate { get; set; }
        public DateOnly? LastDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Description { get; set; }
        public string? SubCategoryName { get; set; }
        public decimal? TakenAmount { get; set; }
        public decimal? GivenAmount { get; set; }
        public decimal? TotalAmount { get; set; }

    }
}
