namespace FMS_Collection.Core.Entities
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string AssetType { get; set; } = string.Empty;
        public string UploadedFileName { get; set; } = string.Empty;
        public string OriginalPath { get; set; } = string.Empty;
        public string? ThumbnailPath { get; set; }
        public string? PreviewPath { get; set; }
        public string? ContentType { get; set; }
        public bool? IsNonSecuredFile { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
