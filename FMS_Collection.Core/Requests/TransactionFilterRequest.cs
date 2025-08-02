namespace FMS_Collection.Core.Request
{
    public class TransactionFilterRequest
    {        
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal? MinAmount{ get; set; }
        public decimal? MaxAmount { get; set; }
        public string? SourceOrReason { get; set; }
    }
}
