namespace FMS_Collection.Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? EmailAddr { get; set; }
        public string? UserName { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? PasswordLastChangedOn { get; set; }
        public DateTime? EffectiveDt { get; set; }
        public DateTime? ExpirationDt { get; set; }
        public Guid? ClientGroupCountry { get; set; }
        public long? FailedLoginCount { get; set; }
        public DateTime? LockExpiryDate { get; set; }
        public Guid? SpecialOccasionId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
