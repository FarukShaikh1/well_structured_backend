namespace FMS_Collection.Core.Request
{
    public class SendOtpRequest
    {
        public string EmailId { get; set; }
        public string Purpose { get; set; } = "login"; // login, password_reset
    }

    public class VerifyOtpRequest
    {
        public string EmailId { get; set; }
        public string OtpCode { get; set; } = string.Empty;
        public string Purpose { get; set; } = "login";
    }

    public class ResetPasswordWithOtpRequest
    {
        public string EmailId { get; set; }
        public string OtpCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}


