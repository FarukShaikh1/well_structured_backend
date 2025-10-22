namespace FMS_Collection.Core.Entities
{
    public class OccasionType
    {
        public Guid? Id { get; set; }
        public string? OccasionTypeName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }

;
}
