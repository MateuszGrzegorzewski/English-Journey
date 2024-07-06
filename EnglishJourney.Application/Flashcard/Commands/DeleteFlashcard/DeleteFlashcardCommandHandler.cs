using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.DeleteFlashcard
{
    public class DeleteFlashcardCommandHandler(IFlashcardRepository repository, ILogger<DeleteFlashcardCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<DeleteFlashcardCommand>
    {
        public async Task Handle(DeleteFlashcardCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting flashcard with id: {FlashcardId}", request.Id);

            var flashcard = await repository.GetFlashardById(request.Id);
            if (flashcard == null) throw new NotFoundException(nameof(Domain.Entities.Flashcard), request.Id.ToString());

            if (!authorizationService.AuthorizeFlashcard(flashcard.FlashcardBox.FlashcardCategory, ResourceOperation.Delete))
                throw new ForbidException();

            await repository.DeleteFlashcard(flashcard);
        }
    }
}