namespace FMS_Collection.Core.Entities
{
    public class CommonList
    {
        public Guid CommonListId { get; set; }
        public string CommonListName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DependantOn { get; set; }
    }
}
