namespace FMS_Collection.Core.Request
{
    public class TransactionRequest
    {
        public Guid? Id { get; set; }             // If you're generating it from .NET (or remove if handled by DB)
        public Guid? AccountId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public decimal? Amount { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? Purpose { get; set; }
    }

}
