using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Connection.Commands.DeleteAttribute
{
    public class DeleteAttributeCommandHandler(IConnectionRepository repository, ILogger<DeleteAttributeCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService) : IRequestHandler<DeleteAttributeCommand>
    {
        public async Task Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting attribute with id: {AttributeId}", request.Id);

            var attribute = await repository.GetAttributeById(request.Id);
            if (attribute == null) throw new NotFoundException(nameof(ConnectionAttribute), request.Id.ToString());

            if (!authorizationService.AuthorizeConnection(attribute.Topic, ResourceOperation.Delete))
                throw new ForbidException();

            await repository.DeleteAttribute(attribute);
        }
    }
}