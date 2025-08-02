using FMS_Collection.Core.Common;
using System;

namespace FMS_Collection.Core.Response
{
    public class TransactionSummaryResponse : CommonResponse
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Purpose { get; set; }
        public string? Description { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Balance { get; set; }
    }
}
