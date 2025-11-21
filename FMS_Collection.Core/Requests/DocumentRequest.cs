using Microsoft.AspNetCore.Http;
using System;

namespace FMS_Collection.Core.Request
{
    public class DocumentRequest
    {
        public IFormFile file { get; set; }

        public Guid? Id { get; set; }
        public string DocumentName { get; set; }
        public string? Keywords { get; set; }

        // Will be assigned after file upload
        public Guid? AssetId { get; set; }
    }
}
