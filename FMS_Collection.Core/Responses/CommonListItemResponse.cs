namespace FMS_Collection.Core.Response
{
    public class CommonListItemResponse 
    {
        public Guid Id { get; set; }
        public string CommonListName { get; set; } = string.Empty;
        public string ListItemName { get; set; } = string.Empty;
        public string? ListItemDescription { get; set; }
        public int? SequenceNumber { get; set; }
    }
}
