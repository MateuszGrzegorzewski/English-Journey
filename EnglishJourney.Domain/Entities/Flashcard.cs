namespace EnglishJourney.Domain.Entities
{
    public class Flashcard
    {
        public int Id { get; set; }
        public string Phrase { get; set; } = default!;
        public string Definition { get; set; } = default!;
        public DateTime LastGuessed { get; set; } = DateTime.UtcNow;

        public int FlashcardBoxId { get; set; }
        public FlashcardBox FlashcardBox { get; set; } = null!;
    }
}