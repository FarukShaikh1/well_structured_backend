namespace FMS_Collection.Core.Response
{
    public class PermissionResponse
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class AuditLogResponse
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
