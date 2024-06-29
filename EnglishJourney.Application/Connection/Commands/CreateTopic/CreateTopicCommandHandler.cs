using AutoMapper;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionTopic
{
    public class CreateTopicCommandHandler(IConnectionRepository repository, IMapper mapper, ILogger<CreateTopicCommandHandler> logger)
        : IRequestHandler<CreateTopicCommand, int>
    {
        public async Task<int> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating topic {@ConnectionTopic}", request);

            var topic = mapper.Map<ConnectionTopic>(request);

            var id = await repository.CreateTopic(topic);
            return id;
        }
    }
}