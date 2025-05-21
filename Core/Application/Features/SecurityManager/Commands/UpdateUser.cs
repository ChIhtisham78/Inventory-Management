using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;


public class UpdateUserResult
{
    public UpdateUserResultDto? Data { get; set; }
}

//public class UpdateUserRequest : IRequest<UpdateUserResult>
//{
//    public string? UserId { get; init; }
//    public string? FirstName { get; init; }
//    public string? LastName { get; init; }
//    public bool? EmailConfirmed { get; init; }
//    public bool? IsBlocked { get; init; }
//    public bool? IsDeleted { get; init; }
//    public string? UpdatedById { get; init; }
//}

public class UpdateUserValidator : AbstractValidator<UpdateUserAsyncDTO>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserAsyncDTO, UpdateUserResult>
{
    private readonly ISecurityService _securityService;

    public UpdateUserHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<UpdateUserResult> Handle(UpdateUserAsyncDTO Dto, CancellationToken cancellationToken)
    {
         Dto.CancellationToken = cancellationToken;
        var result = await _securityService.UpdateUserAsync(Dto);

        return new UpdateUserResult
        {
            Data = result
        };
    }
}

