using AutoMapper;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Queries.GetByTopicId
{
    public class GetTopicByIdQueryHandler(IConnectionRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService,
        ILogger<GetTopicByIdQueryHandler> logger)
        : IRequestHandler<GetTopicByIdQuery, ConnectionTopicDto>
    {
        public async Task<ConnectionTopicDto> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting topic {TopicId}", request.Id);

            var topic = await repository.GetTopicById(request.Id);
            if (topic == null) throw new NotFoundException(nameof(ConnectionTopic), request.Id.ToString());
            var dto = mapper.Map<ConnectionTopicDto>(topic);

            if (!authorizationService.AuthorizeConnection(topic, ResourceOperation.Read))
                throw new ForbidException();

            return dto;
        }
    }
}