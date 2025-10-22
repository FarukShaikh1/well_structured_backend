using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class SpecialOccasionListResponse : CommonResponse
    {
        public Guid Id { get; set; }
        public DateTime? SpecialOccasionDate { get; set; }
        public string? PersonName { get; set; }
        public string? EmailId { get; set; }
        public string? Address { get; set; }
        public string? ImagePath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? RelationShipName { get; set; }
        public string? DayType { get; set; }
        public string? MobileNumber { get; set; }
    }
}
