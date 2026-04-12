// Core/Entities/FamilyPerson.cs
namespace FMS_Collection.Core.Entities
{
    public class FamilyPerson
    {
        public Guid   PersonId         { get; set; }
        public string FirstName        { get; set; } = string.Empty;
        public string LastName         { get; set; } = string.Empty;
        public string FullName         { get; set; } = string.Empty;
        public DateTime? DateOfBirth   { get; set; }
        public char?  Gender           { get; set; }
        public string? Notes           { get; set; }
        public string? ProfileImagePath { get; set; }
        public Guid?  LinkedUserId     { get; set; }
        public Guid   CreatedBy        { get; set; }
        public DateTime CreatedOn      { get; set; }
        public Guid?  UpdatedBy        { get; set; }
        public DateTime? UpdatedOn     { get; set; }
        public bool   IsActive         { get; set; } = true;
    }
}
