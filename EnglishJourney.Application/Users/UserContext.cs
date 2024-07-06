using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EnglishJourney.Application.Users
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("User context does not exist");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;

            return new CurrentUser(userId, email, roles, nationality);
        }
    }
}