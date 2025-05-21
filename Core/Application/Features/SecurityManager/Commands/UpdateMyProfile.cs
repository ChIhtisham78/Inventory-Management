using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;



public class UpdateMyProfileResult
{
    public string? Data { get; init; }
}

public class UpdateMyProfileValidator : AbstractValidator<UpdateUserProfileDTO>
{
    public UpdateMyProfileValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}

public class UpdateMyProfileHandler : IRequestHandler<UpdateUserProfileDTO, UpdateMyProfileResult>
{
    private readonly ISecurityService _securityService;

    public UpdateMyProfileHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<UpdateMyProfileResult> Handle(UpdateUserProfileDTO profileDTO,CancellationToken cancellationToken)
    {
        profileDTO.CancellationToken = cancellationToken;
        await _securityService.UpdateMyProfileAsync(profileDTO);

        return new UpdateMyProfileResult
        {
            Data = "Update MyProfile Success"
        };
    }
}
