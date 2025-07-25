using FMS_Collection.Core.Common;
using System;

namespace FMS_Collection.Core.Response
{
    public class ExpenseListResponse : CommonResponse
    {
        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? SourceOrReason { get; set; }             
        public string? Purpose { get; set; }           
        public string? Description { get; set; }
        public string? ModeOfTransaction { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
    }
}
