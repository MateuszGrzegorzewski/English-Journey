namespace EnglishJourney.Application.Connection
{
    public class ConnectionAttributeDto
    {
        public int Id { get; set; }
        public string Word { get; set; } = default!;
        public string? Definition { get; set; }

        public int TopicId { get; set; }
    }
}