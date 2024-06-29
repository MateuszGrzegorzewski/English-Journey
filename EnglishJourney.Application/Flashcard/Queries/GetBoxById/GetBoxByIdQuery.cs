using MediatR;

namespace EnglishJourney.Application.Flashcard.Queries.GetBoxById
{
    public class GetBoxByIdQuery(int id) : IRequest<FlashcardBoxDto>
    {
        public int Id { get; } = id;
    }
}