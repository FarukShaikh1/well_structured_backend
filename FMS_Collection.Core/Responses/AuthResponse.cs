namespace FMS_Collection.Core.Response
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; }
        public string? RoleName { get; set; }
        public Guid? RoleId { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string? ImagePathSasUrl { get; set; }
        public string? ThumbnailPathSasUrl { get; set; }
        public bool IsOtpRequired { get; set; }
        public DateTime? SpecialOccasionDate { get; set; }
    }
}
