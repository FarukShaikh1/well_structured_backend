namespace FMS_Collection.Core.Response
{
    public class TransactionListResponse
    {
        public Guid? Id { get; set; }
        public Guid? TransactionGroupId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? SourceOrReason { get; set; }
        public string? AccountName { get; set; }
        public decimal? Income { get; set; }
        public decimal? Expense { get; set; }
        public string? Description { get; set; }
        public string? Purpose { get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public Guid? CreatedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        //public Guid? ModifiedBy { get; set; }
        //public bool? IsDeleted { get; set; }
    }
}
