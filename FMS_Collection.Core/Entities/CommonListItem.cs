namespace FMS_Collection.Core.Entities
{
    public class CommonListItem
    {
        public Guid CommonListItemId { get; set; }
        public Guid? CommonListId { get; set; }
        public string ListItemName { get; set; } = string.Empty;
        public string? ListItemDescription { get; set; }
        public int SequenceNumber { get; set; }
        public long? DependantId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
