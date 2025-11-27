namespace FMS_Collection.Core.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string? Keywords { get; set; }
        public Guid AssetId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
