using MediatR;

namespace EnglishJourney.Application.Users.Commands.AddUserRole
{
    public class AddUserRoleCommand : IRequest
    {
        public string UserEmail { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }
}