using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services.SecurityManager
{
    public class UploadFileAsyncDTO
    {
        public string? originalFileName { get; set; }
        public string? docExtension { get; set; }
        public byte[]? fileData { get; set; }
        public long? size { get; set; }
        public string? description { get; set; } = "";
        public string? createdById { get; set; }
        public CancellationToken cancellationToken { get; set; } = default;
    }
}
