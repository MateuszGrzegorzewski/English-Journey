using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.CreateFlashcard
{
    public class CreateFlashcardCommandHandler(IFlashcardRepository repository, IMapper mapper, ILogger<CreateFlashcardCommandHandler> logger)
        : IRequestHandler<CreateFlashcardCommand>
    {
        public async Task Handle(CreateFlashcardCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating flashcard {@Flashcard}", request);

            var flashcard = mapper.Map<Domain.Entities.Flashcard>(request);

            await repository.CreateFlashcard(flashcard);
        }
    }
}