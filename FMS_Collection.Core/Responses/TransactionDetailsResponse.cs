namespace FMS_Collection.Core.Response
{
    public class TransactionDetailsResponse
    {
        public Guid? Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? Purpose { get; set; }
        public decimal? Cash { get; set; }
        public decimal? SbiAccount { get; set; }
        public decimal? CbiAccount { get; set; }
        public decimal? Other { get; set; }
        public Guid? AssetId { get; set; }
        public string? Description { get; set; }
    }
}
