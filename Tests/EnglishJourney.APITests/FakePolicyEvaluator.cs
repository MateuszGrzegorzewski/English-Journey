using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace EnglishJourney.APITests
{
    [ExcludeFromCodeCoverage]
    internal class FakePolicyEvaluator : IPolicyEvaluator
    {
        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                     new Claim(ClaimTypes.Email, "test@example.com"),
                    new Claim(ClaimTypes.Role, "Admin"),
                }, "Test"));

            var ticket = new AuthenticationTicket(claimsPrincipal, "Test");
            var result = AuthenticateResult.Success(ticket);

            context.User = claimsPrincipal;
            return Task.FromResult(result);
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
        {
            var result = PolicyAuthorizationResult.Success();
            return Task.FromResult(result);
        }
    }
}