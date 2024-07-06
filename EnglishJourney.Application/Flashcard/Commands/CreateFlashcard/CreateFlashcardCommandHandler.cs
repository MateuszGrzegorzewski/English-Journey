using AutoMapper;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.CreateFlashcard
{
    public class CreateFlashcardCommandHandler(IFlashcardRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService,
        ILogger<CreateFlashcardCommandHandler> logger)
        : IRequestHandler<CreateFlashcardCommand>
    {
        public async Task Handle(CreateFlashcardCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating flashcard {@Flashcard}", request);

            if (!request.FlashcardBoxId.HasValue)
                throw new NotFoundException(nameof(FlashcardBox), request.ToString() ?? "Null request to create flashcard");

            var boxId = await repository.GetFlashardBoxById(request.FlashcardBoxId.Value);
            if (boxId == null)
                throw new NotFoundException(nameof(FlashcardBox), request.FlashcardBoxId.Value.ToString());

            var flashcard = mapper.Map<Domain.Entities.Flashcard>(request);

            if (!authorizationService.AuthorizeFlashcard(boxId.FlashcardCategory, ResourceOperation.Create))
                throw new ForbidException();

            await repository.CreateFlashcard(flashcard);
        }
    }
}