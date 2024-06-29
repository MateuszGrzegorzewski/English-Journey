using MediatR;

namespace EnglishJourney.Application.Connection.Queries.GetByTopicId
{
    public class GetTopicByIdQuery(int id) : IRequest<ConnectionTopicDto>
    {
        public int Id { get; } = id;
    }
}