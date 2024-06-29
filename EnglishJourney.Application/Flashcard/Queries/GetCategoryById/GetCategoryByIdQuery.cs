using MediatR;

namespace EnglishJourney.Application.Flashcard.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery(int id) : IRequest<FlashcardCategoryDto>
    {
        public int Id { get; } = id;
    }
}