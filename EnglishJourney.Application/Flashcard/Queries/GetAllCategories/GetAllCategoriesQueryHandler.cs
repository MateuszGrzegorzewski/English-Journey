using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler(IFlashcardRepository repository, IMapper mapper, ILogger<GetAllCategoriesQueryHandler> logger)
        : IRequestHandler<GetAllCategoriesQuery, IEnumerable<FlashcardCategoryDto>>
    {
        public async Task<IEnumerable<FlashcardCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all flashcard categories");

            var categories = await repository.GetAllFlashcardCategories();
            var dtos = mapper.Map<IEnumerable<FlashcardCategoryDto>>(categories);

            return dtos;
        }
    }
}