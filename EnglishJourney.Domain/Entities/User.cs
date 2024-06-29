using Microsoft.AspNetCore.Identity;

namespace EnglishJourney.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Nationality { get; set; }
    }
}