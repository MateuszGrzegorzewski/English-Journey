using AutoMapper;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute
{
    public class CreateAttributeCommandHandler(IConnectionRepository repository, IMapper mapper,
        ILogger<CreateAttributeCommandHandler> logger, IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<CreateAttributeCommand, int>
    {
        public async Task<int> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating attribute {@ConnectionAttribute}", request);

            var topic = await repository.GetTopicById(request.TopicId);
            if (topic == null) throw new NotFoundException(nameof(ConnectionTopic), request.TopicId.ToString());

            var attribute = mapper.Map<ConnectionAttribute>(request);

            if (!authorizationService.AuthorizeConnection(topic, ResourceOperation.Delete))
                throw new ForbidException();

            return await repository.CreateAttribute(attribute);
        }
    }
}