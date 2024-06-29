using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Queries.GetAllConnectionTopics
{
    public class GetAllTopicsQueryHandler(IConnectionRepository repository, IMapper mapper, ILogger<GetAllTopicsQueryHandler> logger)
        : IRequestHandler<GetAllTopicsQuery, IEnumerable<ConnectionTopicDto>>
    {
        public async Task<IEnumerable<ConnectionTopicDto>> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all topics with attributes");

            var connectionTopics = await repository.GetAllTopics();
            var dtos = mapper.Map<IEnumerable<ConnectionTopicDto>>(connectionTopics);

            return dtos;
        }
    }
}