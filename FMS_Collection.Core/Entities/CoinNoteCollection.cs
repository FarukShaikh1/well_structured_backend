namespace FMS_Collection.Core.Entities
{
    public class CoinNoteCollection
    {
        public Guid Id { get; set; }
        public string CoinNoteName { get; set; } = string.Empty;
        public Guid? CurrencyTypeId { get; set; }
        public int? CountryId { get; set; }
        public string? MetalsUsed { get; set; }
        public decimal? CoinWeightInGrams { get; set; }
        public decimal? ActualValue { get; set; }
        public decimal? IndianValue { get; set; }
        public string? PrintedYear { get; set; }
        public string? Speciality { get; set; }
        public decimal? DiameterOfCoin { get; set; }
        public decimal? LengthOfNote { get; set; }
        public decimal? BreadthOfNote { get; set; }
        public string? Description { get; set; }
        public Guid AssetId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsVerified { get; set; }
    }
}
