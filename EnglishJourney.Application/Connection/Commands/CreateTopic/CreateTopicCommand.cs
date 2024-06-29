using MediatR;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionTopic
{
    public class CreateTopicCommand : ConnectionTopicDto, IRequest<int>
    {
    }
}