using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class TransactionListResponse : CommonResponse
    {
        public Guid? Id { get; set; }
        public Guid? TransactionGroupId { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? AccountName { get; set; }
        public decimal? Income { get; set; }
        public decimal? Expense { get; set; }
        public string? Description { get; set; }
        public string? Purpose { get; set; }
    }
}
