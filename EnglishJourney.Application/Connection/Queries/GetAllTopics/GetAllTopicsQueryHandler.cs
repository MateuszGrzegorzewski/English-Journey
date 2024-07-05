using AutoMapper;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Queries.GetAllConnectionTopics
{
    public class GetAllTopicsQueryHandler(IConnectionRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService,
        ILogger<GetAllTopicsQueryHandler> logger, IUserContext userContext)
        : IRequestHandler<GetAllTopicsQuery, IEnumerable<ConnectionTopicDto>>
    {
        public async Task<IEnumerable<ConnectionTopicDto>> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all topics with attributes");

            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var connectionTopics = await repository.GetAllTopics(currentUser.Id);
            var dtos = mapper.Map<IEnumerable<ConnectionTopicDto>>(connectionTopics);

            if (connectionTopics is not null && !authorizationService.AuthorizeConnection(connectionTopics.ToList()[0], ResourceOperation.Read))
                throw new ForbidException();

            return dtos;
        }
    }
}