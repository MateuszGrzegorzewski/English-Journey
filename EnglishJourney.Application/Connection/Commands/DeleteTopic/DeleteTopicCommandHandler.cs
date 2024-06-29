using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Commands.DeleteTopic
{
    public class DeleteTopicCommandHandler(IConnectionRepository repository, ILogger<DeleteTopicCommandHandler> logger) : IRequestHandler<DeleteTopicCommand>
    {
        public async Task Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting topic with id: {TopicId}", request.Id);

            var topic = await repository.GetTopicById(request.Id);
            if (topic == null) throw new NotFoundException(nameof(ConnectionTopic), request.Id.ToString());

            await repository.DeleteTopic(topic);
        }
    }
}