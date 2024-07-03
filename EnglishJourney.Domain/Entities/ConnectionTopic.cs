namespace EnglishJourney.Domain.Entities
{
    public class ConnectionTopic
    {
        public int Id { get; set; }
        public string Topic { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public ICollection<ConnectionAttribute> Attributes { get; } = new List<ConnectionAttribute>();

        public User User { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}