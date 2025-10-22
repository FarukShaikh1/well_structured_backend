using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class TransactionReportResponse : CommonResponse
    {
        public DateTime? FirstDate { get; set; }
        public DateTime? LastDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Description { get; set; }
        public decimal? TakenAmount { get; set; }
        public decimal? GivenAmount { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
