namespace FMS_Collection.Core.Response
{
    public class NotificationSummaryResponse
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencySymbol { get; set; }
        public int? NumberOfNotes { get; set; }
        public int? NumberOfCoins { get; set; }
        public int? Total { get; set; }

    }
}
