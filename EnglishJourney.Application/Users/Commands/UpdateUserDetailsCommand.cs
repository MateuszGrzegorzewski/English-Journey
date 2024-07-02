using MediatR;

namespace EnglishJourney.Application.Users.Commands
{
    public class UpdateUserDetailsCommand : IRequest
    {
        public string? Nationality { get; set; }
    }
}