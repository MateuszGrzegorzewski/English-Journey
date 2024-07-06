using AutoMapper;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionTopic
{
    public class CreateTopicCommandHandler(IConnectionRepository repository, IMapper mapper,
        ILogger<CreateTopicCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService,
        IUserContext userContext) : IRequestHandler<CreateTopicCommand, int>
    {
        public async Task<int> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User is not authenticated");

            logger.LogInformation("{UserEmail} [{UserId}] is creating topic {@ConnectionTopic}",
                currentUser.Email, currentUser.Id, request);

            var topic = mapper.Map<ConnectionTopic>(request);
            topic.UserId = currentUser.Id;

            if (!authorizationService.AuthorizeConnection(topic, ResourceOperation.Create))
                throw new ForbidException();

            var id = await repository.CreateTopic(topic);
            return id;
        }
    }
}