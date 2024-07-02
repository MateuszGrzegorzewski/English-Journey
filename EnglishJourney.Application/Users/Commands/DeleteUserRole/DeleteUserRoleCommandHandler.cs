using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishJourney.Application.Users.Commands.DeleteUserRole
{
    internal class DeleteUserRoleCommandHandler(ILogger<DeleteUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<DeleteUserRoleCommand>
    {
        public async Task Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Removing role {RoleName} from user {UserEmail}", request.RoleName, request.UserEmail);

            var dbUser = await userManager.FindByEmailAsync(request.UserEmail)
                 ?? throw new NotFoundException(nameof(User), request.UserEmail);

            var role = await roleManager.FindByNameAsync(request.RoleName)
                ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

            await userManager.RemoveFromRoleAsync(dbUser, role.Name!);
        }
    }
}