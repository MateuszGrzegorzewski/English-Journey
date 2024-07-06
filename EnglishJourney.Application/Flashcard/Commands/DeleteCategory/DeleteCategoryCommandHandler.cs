using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IFlashcardRepository repository, ILogger<DeleteCategoryCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<DeleteCategoryCommand>
    {
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting flashcard category with id: {FlashcardId}", request.Id);

            var category = await repository.GetFlashardCategoryById(request.Id);
            if (category == null) throw new NotFoundException(nameof(FlashcardCategory), request.Id.ToString());

            if (!authorizationService.AuthorizeFlashcard(category, ResourceOperation.Delete))
                throw new ForbidException();

            await repository.DeleteFlashcardCategory(category);
        }
    }
}