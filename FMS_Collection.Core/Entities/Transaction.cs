namespace FMS_Collection.Core.Entities
{
    public class Transaction
    {
        public Guid? Id { get; set; }
        public Guid? AccountId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public Guid? AssetId { get; set; }
        public string? Purpose { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
