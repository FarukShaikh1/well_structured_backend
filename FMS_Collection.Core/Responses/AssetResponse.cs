using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class AssetResponse : CommonResponse
    {        
        public Guid Id { get; set; }
        public string? AssetType { get; set; }
        public string? UploadedFileName { get; set; }
        public string? OriginalPath { get; set; }
        public string? ThumbnailPath { get; set; }        
        public string? PreviewPath { get; set; }
        public string? ContentType { get; set; }
        public bool? IsNonSecuredFile { get; set; }
    }
}
