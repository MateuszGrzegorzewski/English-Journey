using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Commands.EditConnectionTopic
{
    public class EditTopicCommandHandler(IConnectionRepository repository, ILogger<EditTopicCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService) : IRequestHandler<EditTopicCommand>
    {
        public async Task Handle(EditTopicCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating topic with id: {TopicId} with {@EditedConnectionTopic}", request.Id, request);

            var topic = await repository.GetTopicById(request.Id);
            if (topic == null) throw new NotFoundException(nameof(ConnectionTopic), request.Id.ToString());

            if (!authorizationService.AuthorizeConnection(topic, ResourceOperation.Update))
                throw new ForbidException();

            topic.Topic = request.Topic;
            topic.LastModified = DateTime.UtcNow;

            await repository.Commit();
        }
    }
}