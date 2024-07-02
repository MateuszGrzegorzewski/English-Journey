using MediatR;

namespace EnglishJourney.Application.Users.Commands.UpdateUserDetails
{
    public class UpdateUserDetailsCommand : IRequest
    {
        public string? Nationality { get; set; }
    }
}