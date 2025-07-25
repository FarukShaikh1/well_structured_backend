namespace FMS_Collection.Core.Entities
{
    public class ExpenseAuditForLog
    {
        public long ExpenseAuditId { get; set; }
        public Guid? ExpenseId { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string? SourceOrReason { get; set; }
        public decimal? Cash { get; set; }
        public decimal? SBIAccount { get; set; }
        public decimal? CBIAccount { get; set; }
        public decimal? Other { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? CashBalance { get; set; }
        public decimal? SBIBalance { get; set; }
        public decimal? CBIBalance { get; set; }
        public decimal? OtherBalance { get; set; }
        public decimal? TotalAvailable { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNumber { get; set; }
        public bool? IsInvoiceAvailable { get; set; }
        public int? TransactionStatusId { get; set; }
        public string? Operation { get; set; }
        public DateTime? AuditCreateDate { get; set; }
        public string? Purpose { get; set; }
        public Guid? AssetId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
