using Application.Common.Services.SecurityManager;

namespace Application.Common.Services.FileImageManager;
public interface IFileImageService
{
    Task<string> UploadAsync(UploadFileAsyncDTO uploadFile);
    Task<byte[]> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
}
