namespace FMS_Collection.Core.Entities
{
    public class CountryWithCurrency
    {
        public long Id { get; set; }
        public string Country { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public int? DialCode { get; set; }
        public int? DisplaySequence { get; set; }
        public string? Nationality { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
