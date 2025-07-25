namespace FMS_Collection.Core.Entities
{
    public class UserRole
    {
        public long Id { get; set; }
        public bool? IsDelete { get; set; }
        public Guid UserRoleId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
    }
}
