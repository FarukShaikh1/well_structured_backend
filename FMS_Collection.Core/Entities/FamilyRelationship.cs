// Core/Entities/FamilyRelationship.cs
namespace FMS_Collection.Core.Entities
{
    /// <summary>
    /// Valid relationship types stored in the database.
    /// Bidirectional rules are enforced by the FamilyRelationship_Add stored procedure.
    /// </summary>
    public static class RelationshipType
    {
        public const string Father  = "Father";
        public const string Mother  = "Mother";
        public const string Child   = "Child";
        public const string Sibling = "Sibling";
        public const string Spouse  = "Spouse";

        public static readonly IReadOnlyList<string> All =
            [Father, Mother, Child, Sibling, Spouse];
    }

    public class FamilyRelationship
    {
        public Guid   RelationshipId   { get; set; }
        public Guid   PersonId         { get; set; }
        public Guid   RelatedPersonId  { get; set; }
        public string RelationshipType { get; set; } = string.Empty;
        public Guid   CreatedBy        { get; set; }
        public DateTime CreatedOn      { get; set; }
        public bool   IsActive         { get; set; } = true;
    }
}
