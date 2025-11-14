namespace FMS_Collection.Core.Request
{
    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public Guid? UserId{ get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
