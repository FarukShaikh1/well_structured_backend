// Core/Request/FamilyPersonRequest.cs
namespace FMS_Collection.Core.Request
{
    public class FamilyPersonRequest
    {
        public Guid?   PersonId         { get; set; }  // null = create new
        public string  FirstName        { get; set; } = string.Empty;
        public string  LastName         { get; set; } = string.Empty;
        public DateTime? DateOfBirth    { get; set; }
        public char?   Gender           { get; set; }
        public string? Notes            { get; set; }
        public string? ProfileImagePath { get; set; }
        public Guid?   LinkedUserId     { get; set; }
    }

    public class FamilyRelationshipRequest
    {
        public Guid   PersonId         { get; set; }
        public Guid   RelatedPersonId  { get; set; }
        /// <summary>Father | Mother | Child | Sibling | Spouse</summary>
        public string RelationshipType { get; set; } = string.Empty;
    }

    public class FamilyRelationshipDeleteRequest
    {
        public Guid RelationshipId { get; set; }
    }
}
