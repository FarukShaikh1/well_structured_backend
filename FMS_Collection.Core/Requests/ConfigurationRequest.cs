namespace FMS_Collection.Core.Request
{
    public class ConfigurationRequest
    {
        public Guid? Id { get; set; }
        public string? ConfigurationName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
        public Guid? UserId { get; set; }
    }

;
}
