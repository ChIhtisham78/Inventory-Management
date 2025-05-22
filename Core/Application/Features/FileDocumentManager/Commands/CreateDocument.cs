using Application.Common.Services.FileDocumentManager;
using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.FileDocumentManager.Commands
{
    public class CreateDocumentResult
    {
        public string? DocumentName { get; init; }
    }

    public class CreateDocumentRequest : IRequest<CreateDocumentResult>
    {
        public string? OriginalFileName { get; init; }
        public string? Extension { get; init; }
        public byte[]? Data { get; init; }
        public long? Size { get; init; }
        public string? CreatedById { get; init; }
        public string? Description { get; init; }
    }

    public class CreateDocumentValidator : AbstractValidator<CreateDocumentRequest>
    {
        public CreateDocumentValidator()
        {
            RuleFor(x => x.OriginalFileName).NotEmpty();
            RuleFor(x => x.Extension).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
            RuleFor(x => x.Size).NotEmpty();
        }
    }

    public class CreateDocumentHandler : IRequestHandler<CreateDocumentRequest, CreateDocumentResult>
    {
        private readonly IFileDocumentService _uploadDocument;

        public CreateDocumentHandler(IFileDocumentService uploadDocument)
        {
            _uploadDocument = uploadDocument;
        }

        public async Task<CreateDocumentResult> Handle(CreateDocumentRequest request, CancellationToken cancellationToken)
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

            var result = await _uploadDocument.UploadAsync(dto);

            return new CreateDocumentResult { DocumentName = result };
        }
    }
}
