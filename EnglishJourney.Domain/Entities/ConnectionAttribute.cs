namespace EnglishJourney.Domain.Entities
{
    public class ConnectionAttribute
    {
        public int Id { get; set; }
        public string Word { get; set; } = default!;
        public string? Definition { get; set; }

        public int TopicId { get; set; }
        public ConnectionTopic Topic { get; set; } = default!;
    }
}