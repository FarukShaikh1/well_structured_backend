namespace FMS_Collection.Core.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public bool IsGranted { get; set; } = true;
        public DateTime AssignedOn { get; set; }
        public Guid AssignedBy { get; set; }
    }
}
