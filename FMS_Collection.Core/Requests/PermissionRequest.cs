namespace FMS_Collection.Core.Request
{
    public class PermissionRequest
    {
        public string PermissionName { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class AssignRolePermissionsRequest
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } = [];
    }

    public class AssignUserRoleRequest
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
