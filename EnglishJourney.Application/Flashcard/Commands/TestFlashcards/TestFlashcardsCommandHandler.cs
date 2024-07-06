using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.TestFlashcards
{
    public class TestFlashcardsCommandHandler(IFlashcardRepository repository, ILogger<TestFlashcardsCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<TestFlashcardsCommand>
    {
        public async Task Handle(TestFlashcardsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Testing flashcards {@FlashcardsTest}", request);

            if (request.TestResults == null)
            {
                throw new ArgumentNullException(nameof(request.TestResults));
            }

            foreach (var testResult in request.TestResults)
            {
                var flashcardId = testResult.Key;
                var knowsWord = testResult.Value;

                var flashcard = await repository.GetFlashardById(flashcardId);
                if (flashcard == null)
                {
                    logger.LogError("Flashcard with id {FlashcardId} not found", flashcardId);
                    continue;
                }

                if (!authorizationService.AuthorizeFlashcard(flashcard.FlashcardBox.FlashcardCategory, ResourceOperation.Update))
                    throw new ForbidException();

                if (knowsWord)
                {
                    var box = await repository.GetFlashardBoxById(flashcard.FlashcardBoxId);
                    if (box == null) throw new NotFoundException(nameof(FlashcardBox), flashcard.FlashcardBoxId.ToString());
                    var nextBox = await repository.GetFlashcardBoxByCategoryIdAndBoxNumber(box.FlashcardCategoryId, box.BoxNumber + 1);

                    if (nextBox != null)
                    {
                        flashcard.FlashcardBoxId = nextBox.Id;
                    }
                }

                flashcard.LastGuessed = DateTime.UtcNow;
            }

            await repository.Commit();
        }
    }
}