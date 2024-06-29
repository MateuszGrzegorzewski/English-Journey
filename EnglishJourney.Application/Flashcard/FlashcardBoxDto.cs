using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Application.Flashcard
{
    public class FlashcardBoxDto
    {
        public int Id { get; set; }
        public int BoxNumber { get; set; }

        public ICollection<FlashcardDto> Flashcards { get; } = new List<FlashcardDto>();
    }
}