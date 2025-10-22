
using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class CountryResponse : CommonResponse
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CountryCode { get; set; }
        public string DialCode { get; set; }
        public string Nationality { get; set; }
    }
}
