namespace EnglishJourney.Application.Flashcard
{
    public class FlashcardCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public ICollection<FlashcardBoxDto> FlashcardBoxes { get; } = new List<FlashcardBoxDto>();
    }
}