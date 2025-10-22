namespace FMS_Collection.Core.Response
{
    public class NotificationListResponse
    {
        public Guid? Id { get; set; }
        public string? CurrencyCoinType { get; set; }
        public string? CoinNoteName { get; set; }
        public string? CountryName { get; set; }
        public string? MetalsUsed { get; set; }
        public decimal? CoinWeightInGrams { get; set; }
        public decimal? ActualValue { get; set; }
        public decimal? IndianValue { get; set; }
        public string? PrintedYear { get; set; }
        public decimal?  DiameterOfCoin{ get; set; }
        public decimal? LengthOfNote { get; set; }
        public decimal? BreadthOfNote { get; set; }
        public string? Speciality { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? RupeeSymbol { get; set; }
        public bool? IsVerified { get; set; }
    }
}
