using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.EditCategory
{
    public class EditCategoryCommandHandler(IFlashcardRepository repository, ILogger<EditCategoryCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<EditCategoryCommand>
    {
        public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Editing flashcard category {@FlashcardCategory} with id: {FlashcardCategoryId}", request, request.Id);

            var category = await repository.GetFlashardCategoryById(request.Id);
            if (category == null) throw new NotFoundException(nameof(Domain.Entities.FlashcardCategory), request.Id.ToString());

            category.Name = request.Name;

            if (!authorizationService.AuthorizeFlashcard(category, ResourceOperation.Update))
                throw new ForbidException();

            await repository.Commit();
        }
    }
}