namespace FMS_Collection.Core.Entities
{
    public class Permission
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
