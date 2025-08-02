namespace FMS_Collection.Core.Response
{
    public class ConfigurationResponse
    {
        public Guid? Id { get; set; }
        public string? ConfigurationName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
