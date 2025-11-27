using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
  public class DocumentListResponse
  {
    public Guid? Id { get; set; }
    public string? DocumentName { get; set; }
    public string? Keywords { get; set; }
    public string? DocumentType { get; set; }   // MIME Type
    public long? SizeBytes { get; set; }
    public DateTimeOffset? UploadedOn { get; set; }    // Blob LastModified
    public string? UploadedFileName { get; set; }
    public string? OriginalPath { get; set; }
    public string? OriginalPathSasUrl { get; set; }
    public string? ThumbnailPath { get; set; }
    public string? ThumbnailPathSasUrl { get; set; }
    public string? PreviewPath { get; set; }
  }
}
