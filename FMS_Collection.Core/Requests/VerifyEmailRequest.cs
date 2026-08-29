public class VerifyEmailRequest
{
    public Guid RegistrationId { get; set; }

    public string Otp { get; set; } = string.Empty;
}