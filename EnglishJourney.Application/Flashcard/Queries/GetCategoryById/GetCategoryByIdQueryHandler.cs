using AutoMapper;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler(IFlashcardRepository repository, IMapper mapper, ILogger<GetCategoryByIdQueryHandler> logger)
        : IRequestHandler<GetCategoryByIdQuery, FlashcardCategoryDto>
    {
        public async Task<FlashcardCategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting flashcard category with id: {FlashcardCategoryId}", request.Id);

            var category = await repository.GetFlashardCategoryById(request.Id);
            if (category == null) throw new NotFoundException(nameof(Domain.Entities.FlashcardCategory), request.Id.ToString());

            var dto = mapper.Map<FlashcardCategoryDto>(category);

            return dto;
        }
    }
}