using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SecurityManager.Commands;
using MediatR;

namespace Application.Common.Services.SecurityManager
{
    public class UpdateUserProfileDTO : IRequest<UpdateMyProfileResult>
    {
        public string UserId { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? CompanyName { get; set; }
        public CancellationToken CancellationToken { get; set; } = default;
    }

}
