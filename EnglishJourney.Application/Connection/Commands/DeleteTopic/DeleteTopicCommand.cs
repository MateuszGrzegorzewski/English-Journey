using MediatR;

namespace EnglishJourney.Application.Connection.Commands.DeleteTopic
{
    public class DeleteTopicCommand : ConnectionTopicDto, IRequest
    {
    }
}