namespace EnglishJourney.Application.Connection
{
    public class ConnectionTopicDto
    {
        public int Id { get; set; }
        public string Topic { get; set; } = default!;

        public ICollection<ConnectionAttributeDto> Attributes { get; } = new List<ConnectionAttributeDto>();
    }
}