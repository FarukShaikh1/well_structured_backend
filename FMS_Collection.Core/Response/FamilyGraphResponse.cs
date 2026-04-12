// Core/Response/FamilyGraphResponse.cs
namespace FMS_Collection.Core.Response
{
    /// <summary>A single node (person) in the graph.</summary>
    public class FamilyGraphNode
    {
        public string  Id               { get; set; } = string.Empty;  // Guid as string for vis-network
        public string  Label            { get; set; } = string.Empty;  // FullName
        public string  FirstName        { get; set; } = string.Empty;
        public string  LastName         { get; set; } = string.Empty;
        public DateTime? DateOfBirth    { get; set; }
        public char?   Gender           { get; set; }
        public string? ProfileImagePath { get; set; }
        public int     Depth            { get; set; }
        public bool    IsRoot           { get; set; }
    }

    /// <summary>A directed relationship edge between two persons.</summary>
    public class FamilyGraphEdge
    {
        public string Id    { get; set; } = string.Empty;  // RelationshipId as string
        public string From  { get; set; } = string.Empty;
        public string To    { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;  // RelationshipType
    }

    /// <summary>Complete graph payload returned by the API.</summary>
    public class FamilyGraphResponse
    {
        public string                  RootPersonId { get; set; } = string.Empty;
        public List<FamilyGraphNode>   Nodes        { get; set; } = [];
        public List<FamilyGraphEdge>   Edges        { get; set; } = [];
    }

    public class FamilyPersonSearchResult
    {
        public string  PersonId         { get; set; } = string.Empty;
        public string  FullName         { get; set; } = string.Empty;
        public string  FirstName        { get; set; } = string.Empty;
        public string  LastName         { get; set; } = string.Empty;
        public DateTime? DateOfBirth    { get; set; }
        public char?   Gender           { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
