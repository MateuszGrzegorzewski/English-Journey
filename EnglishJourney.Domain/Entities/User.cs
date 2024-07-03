using Microsoft.AspNetCore.Identity;

namespace EnglishJourney.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Nationality { get; set; }

        public List<Note> Notes { get; set; } = [];
        public List<ConnectionTopic> ConnectionTopics { get; set; } = [];
        public List<FlashcardCategory> FlashcardCategories { get; set; } = [];
    }
}