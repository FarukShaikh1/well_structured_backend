namespace FMS_Collection.Core.Entities
{
    public class SpecialOccasion
    {
        public Guid Id { get; set; }
        public DateTime? specialOccasionDate { get; set; }
        public string? PersonName { get; set; }
        public string? Address { get; set; }
        public int? SuperAdminRelationId { get; set; }
        public string? MobileNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmailId { get; set; }
        public Guid? DayTypeId { get; set; }
        public string? DayTypeName { get; set; }
    public string? RelationName { get; set; }
    public Guid? AssetId { get; set; }
        public char? Gender { get; set; }
        public Guid? RelationId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsRestricted { get; set; }
        public bool? IsVerified { get; set; }
    }
}
