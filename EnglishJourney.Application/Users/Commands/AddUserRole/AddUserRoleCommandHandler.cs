using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Users.Commands.AddUserRole
{
    public class AddUserRoleCommandHandler(ILogger<AddUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<AddUserRoleCommand>
    {
        public async Task Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Adding role {RoleName} to user {UserEmail}", request.RoleName, request.UserEmail);

            var dbUser = await userManager.FindByEmailAsync(request.UserEmail)
                ?? throw new NotFoundException(nameof(User), request.UserEmail);

            var role = await roleManager.FindByNameAsync(request.RoleName)
                ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

            await userManager.AddToRoleAsync(dbUser, role.Name!);
        }
    }
}