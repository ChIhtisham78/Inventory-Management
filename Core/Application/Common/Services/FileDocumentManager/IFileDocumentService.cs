using Application.Common.Services.SecurityManager;

namespace Application.Common.Services.FileDocumentManager;
public interface IFileDocumentService
{
    Task<string> UploadAsync(UploadFileAsyncDTO uploadFile);
    Task<byte[]> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
}