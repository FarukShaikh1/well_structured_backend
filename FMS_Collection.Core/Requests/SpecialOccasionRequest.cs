namespace FMS_Collection.Core.Request
{
    public class SpecialOccasionRequest
    {
        public Guid? Id { get; set; }
        public DateTime? SpecialOccasionDate { get; set; }
        public string? PersonName { get; set; } = string.Empty;
        public Guid? DayTypeId { get; set; }
        public Guid? RelationId { get; set; }
        public string? MobileNumber { get; set; }
        public string? AlternateNumber { get; set; }
        public string? EmailId { get; set; }
        public char? Gender { get; set; } = 'M';
        public string? Address { get; set; }
        public Guid? AssetId { get; set; }
    }
}