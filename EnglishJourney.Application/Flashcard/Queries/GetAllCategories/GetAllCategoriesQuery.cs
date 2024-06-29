using MediatR;

namespace EnglishJourney.Application.Flashcard.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<FlashcardCategoryDto>>
    {
    }
}