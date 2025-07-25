using FMS_Collection.Core.Common;
using System;

namespace FMS_Collection.Core.Response
{
    public class ExpenseSummaryResponse : CommonResponse
    {
        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Purpose { get; set; }
        public string? Description { get; set; }
        public decimal? Cash { get; set; }
        public decimal? SBIAccount { get; set; }
        public decimal? CBIAccount { get; set; }
        public decimal? Other { get; set; }
        public decimal? CashBalance { get; set; }
        public decimal? SBIBalance { get; set; }
        public decimal? CBIBalance { get; set; }
        public decimal? OtherBalance { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalAvailable { get; set; }
    }
}
