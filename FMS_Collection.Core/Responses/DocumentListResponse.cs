using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class DocumentListResponse : CommonResponse
    {
        public Guid? Id { get; set; }
        public string? DocumentName { get; set; }
        public string? Keywords{ get; set; }

        public string? OriginalPath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? PreviewPath { get; set; }
    }
}
