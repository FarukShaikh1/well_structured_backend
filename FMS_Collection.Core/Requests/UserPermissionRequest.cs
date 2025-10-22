namespace FMS_Collection.Core.Request
{
    public class UserPermissionRequest  
    {
        public Guid? UserId { get; set; }
        public Guid? ModuleId { get; set; }
        public bool? View { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? Download { get; set; }
        public bool? Upload { get; set; }
    }
}
