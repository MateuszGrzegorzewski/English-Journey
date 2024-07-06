using AutoMapper;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Queries.GetBoxById
{
    public class GetBoxByIdQueryHandler(IFlashcardRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService,
        ILogger<GetBoxByIdQueryHandler> logger)
        : IRequestHandler<GetBoxByIdQuery, FlashcardBoxDto>
    {
        public async Task<FlashcardBoxDto> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting flashcard box with id: {FlashcardBoxId}", request.Id);

            var box = await repository.GetFlashardBoxById(request.Id);
            if (box == null) throw new NotFoundException(nameof(Domain.Entities.FlashcardBox), request.Id.ToString());

            var dto = mapper.Map<FlashcardBoxDto>(box);

            if (!authorizationService.AuthorizeFlashcard(box.FlashcardCategory, ResourceOperation.Read))
                throw new ForbidException();

            return dto;
        }
    }
}