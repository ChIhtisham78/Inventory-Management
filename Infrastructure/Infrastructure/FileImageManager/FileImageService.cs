using Application.Common.Repositories;
using Application.Common.Services.FileImageManager;
using Application.Common.Services.SecurityManager;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.FileImageManager;

public class FileImageService : IFileImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _folderPath;
    private readonly int _maxFileSizeInBytes;
    private readonly ICommandRepository<FileImage> _docRepository;

    public FileImageService(
        IUnitOfWork unitOfWork,
        IOptions<FileImageSettings> settings,
        ICommandRepository<FileImage> docRepository
        )
    {
        _unitOfWork = unitOfWork;
        _folderPath = Path.Combine(Directory.GetCurrentDirectory(), settings.Value.PathFolder);
        _maxFileSizeInBytes = settings.Value.MaxFileSizeInMB * 1024 * 1024;
        _docRepository = docRepository;
    }

    public async  Task<string> UploadAsync(UploadFileAsyncDTO uploadFile)
    {

        if (string.IsNullOrWhiteSpace(uploadFile.docExtension) || uploadFile.docExtension.Contains(Path.DirectorySeparatorChar) || uploadFile.docExtension.Contains(Path.AltDirectorySeparatorChar))
        {
            throw new Exception($"Invalid file extension: {nameof(uploadFile.docExtension)}");
        }

        if (uploadFile.fileData! == null || uploadFile.fileData!.Length == 0)
        {
            throw new Exception($"File data cannot be null or empty: {nameof(uploadFile.fileData)}");
        }

        if (uploadFile.fileData.Length > _maxFileSizeInBytes)
        {
            throw new Exception($"File size exceeds the maximum allowed size of {_maxFileSizeInBytes / (1024 * 1024)} MB");
        }

        var fileName = $"{Guid.NewGuid():N}.{uploadFile.docExtension}";

        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        var filePath = Path.Combine(_folderPath, fileName);

        await File.WriteAllBytesAsync(filePath, uploadFile.fileData, uploadFile.cancellationToken);

        var img = new FileImage();
        img.Name = fileName;
        img.OriginalName = uploadFile.originalFileName;
        img.Extension = uploadFile.docExtension;
        img.GeneratedName = fileName;
        img.FileSize = uploadFile.size;
        img.Description = uploadFile.description;
        img.CreatedById = uploadFile.createdById;

        await _docRepository.CreateAsync(img, uploadFile.cancellationToken);
        await _unitOfWork.SaveAsync(uploadFile.cancellationToken);

        return fileName;
    }

    public async Task<byte[]> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_folderPath, fileName);

        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "noimage.png");
        }

        var result = await File.ReadAllBytesAsync(filePath, cancellationToken);

        return result;
    }

}
