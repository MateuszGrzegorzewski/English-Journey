namespace EnglishJourney.Domain.Entities
{
    public class FlashcardCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<FlashcardBox> FlashcardBoxes { get; } = new List<FlashcardBox>();

        public User User { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}