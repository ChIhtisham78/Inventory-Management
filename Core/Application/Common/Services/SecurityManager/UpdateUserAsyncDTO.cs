using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SecurityManager.Commands;
using MediatR;

namespace Application.Common.Services.SecurityManager
{
    public class UpdateUserAsyncDTO : IRequest<UpdateUserResult>
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string UpdatedById { get; set; } = string.Empty;
        public CancellationToken CancellationToken { get; set; } = default;
    }
}
