using System;

namespace FMS_Collection.Core.Request
{
    public class NotificationRequest
    {
        public Guid? Id { get; set; }
        public string CoinNoteName { get; set; }
        public Guid CollectionCoinTypeId { get; set; }
        public int? CountryId { get; set; }
        public string? MetalsUsed { get; set; }
        public float? CoinWeightInGrams { get; set; }
        public float? ActualValue { get; set; }
        public float? IndianValue { get; set; }
        public string? PrintedYear { get; set; }
        public string? Speciality { get; set; }
        public decimal? DiameterOfCoin { get; set; }
        public decimal? LengthOfNote { get; set; }
        public decimal? BreadthOfNote { get; set; }
        public string? Description { get; set; }
        public Guid AssetId { get; set; }
    }
}
