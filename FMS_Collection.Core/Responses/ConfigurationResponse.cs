using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class ConfigurationResponse : CommonResponse
    {
        public Guid? Id { get; set; }
        public string? ConfigurationName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
       
    }
}
