namespace EnglishJourney.Domain.Entities
{
    public class FlashcardBox
    {
        public int Id { get; set; }
        public int BoxNumber { get; set; }

        public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

        public int FlashcardCategoryId { get; set; }
        public FlashcardCategory FlashcardCategory { get; set; } = null!;
    }
}