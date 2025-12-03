using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
  public class SpecialOccasionDetailsResponse : CommonResponse
  {
    public Guid? Id { get; set; }
    public DateTime? SpecialOccasionDate { get; set; }
    public string? PersonName { get; set; }
    public Guid? DayTypeId { get; set; }
    public Guid? RelationId { get; set; }
    public string? MobileNumber { get; set; }
    public string? ContactNumber { get; set; }
    public string? EmailId { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public Guid? AssetId { get; set; }
    public string? DayTypeName { get; set; }
    public string? RelationName { get; set; }
  }

}
