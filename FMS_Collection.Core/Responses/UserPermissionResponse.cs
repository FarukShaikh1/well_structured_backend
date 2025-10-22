namespace FMS_Collection.Core.Response
{
    public class UserPermissionResponse
    {
        public Guid? Id { get; set; }
        public Guid? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? Description{ get; set; }
        public string? Route { get; set; }
        public bool? View { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? Download { get; set; }
        public bool? Upload { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
