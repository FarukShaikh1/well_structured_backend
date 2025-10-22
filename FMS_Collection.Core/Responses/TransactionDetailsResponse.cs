using FMS_Collection.Core.Request;

namespace FMS_Collection.Core.Response
{
    public class TransactionDetailsResponse
    {
        public Guid? Id { get; set; }
        public Guid? TransactionGroupId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Description { get; set; }
        public string? Purpose { get; set; }
        public List<TransactionAccountSplit> AccountSplits { get; set; } = new();
    }
}
