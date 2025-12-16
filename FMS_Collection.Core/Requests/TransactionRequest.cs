namespace FMS_Collection.Core.Request
{
    public class TransactionRequest
    {
        public Guid? Id { get; set; }
        public Guid? TransactionGroupId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Description { get; set; }
        public Guid? TransactionCategoryId { get; set; }
        public string? Purpose { get; set; }
        public List<TransactionAccountSplit> AccountSplits { get; set; } = new();
    }

    public class TransactionAccountSplit
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionCategory? Category { get; set; }
    }

    public enum TransactionCategory
    {
        Income,
        Expense
    }

}


