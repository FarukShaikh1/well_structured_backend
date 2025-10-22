namespace FMS_Collection.Core.Response
{
    public class ModuleListResponse
    {
        public Guid Id { get; set; }
        public string? ModuleName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string? Route{ get; set; }
        public int? DisplayOrder{ get; set; }
        public string? IconClass { get; set; }
    }
}
