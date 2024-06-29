using MediatR;

namespace EnglishJourney.Application.Connection.Queries.GetAllConnectionTopics
{
    public class GetAllTopicsQuery : IRequest<IEnumerable<ConnectionTopicDto>>
    {
    }
}