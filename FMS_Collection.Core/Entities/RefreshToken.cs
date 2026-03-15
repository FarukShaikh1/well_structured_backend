namespace FMS_Collection.Core.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsRevoked => RevokedOn.HasValue;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
