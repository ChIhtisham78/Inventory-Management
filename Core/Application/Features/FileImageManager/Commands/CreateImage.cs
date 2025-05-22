using Application.Common.Services.FileImageManager;
using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.FileImageManager.Commands;

public class CreateImageResult
{
    public string? ImageName { get; init; }
}

public class CreateImageRequest : IRequest<CreateImageResult>
{
    public string? OriginalFileName { get; init; }
    public string? Extension { get; init; }
    public byte[]? Data { get; init; }
    public long? Size { get; init; }
    public string? CreatedById { get; init; }
    public string? Description { get; init; }
}

public class CreateImageValidator : AbstractValidator<CreateImageRequest>
{
    public CreateImageValidator()
    {
        RuleFor(x => x.OriginalFileName)
            .NotEmpty();

        RuleFor(x => x.Extension)
            .NotEmpty();

        RuleFor(x => x.Data)
            .NotEmpty();

        RuleFor(x => x.Size)
            .NotEmpty();
    }
}

public class CreateImageHandler : IRequestHandler<CreateImageRequest, CreateImageResult>
{
    private readonly IFileImageService _uploadImage;

    public CreateImageHandler(IFileImageService uploadImage)
    {
        _uploadImage = uploadImage;
    }

    public async Task<CreateImageResult> Handle(CreateImageRequest request, CancellationToken cancellationToken)
    {
        var dto = new UploadFileAsyncDTO
        {
            originalFileName = request.OriginalFileName,
            docExtension = request.Extension,
            fileData = request.Data,
            size = request.Size,
            description = request.Description ?? "",
            createdById = request.CreatedById,
            cancellationToken = cancellationToken
        };
        var result = await _uploadImage.UploadAsync(dto);

        return new CreateImageResult { ImageName = result };
    }
}

