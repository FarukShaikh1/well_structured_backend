using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class TransactionSummaryResponse : CommonResponse
    {
        public Guid TransactionGroupId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Purpose { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, object?> AccountData { get; set; } = new();
    }
}
