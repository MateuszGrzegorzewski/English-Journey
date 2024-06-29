using MediatR;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute
{
    public class CreateAttributeCommand : ConnectionAttributeDto, IRequest<int>
    {
    }
}