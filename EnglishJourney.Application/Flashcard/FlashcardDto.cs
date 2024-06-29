namespace EnglishJourney.Application.Flashcard
{
    public class FlashcardDto
    {
        public int Id { get; set; }
        public string Phrase { get; set; } = default!;
        public string Definition { get; set; } = default!;

        public FlashcardBoxDto? FlashcardBox { get; }
        public int? FlashcardBoxId { get; set; }
    }
}