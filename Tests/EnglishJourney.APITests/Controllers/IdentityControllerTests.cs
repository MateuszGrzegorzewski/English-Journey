using EnglishJourney.Application.Users.Commands.AddUserRole;
using EnglishJourney.Application.Users.Commands.DeleteUserRole;
using EnglishJourney.Application.Users.Commands.UpdateUserDetails;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using Xunit;

namespace EnglishJourney.APITests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class IdentityControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly Mock<IMediator> mediatorMock = new();

        public IdentityControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.Replace(ServiceDescriptor.Scoped(typeof(IMediator),
                                                _ => mediatorMock.Object));
                });
            });
        }

        [Fact]
        public async Task UpdateUserDetails_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var command = new UpdateUserDetailsCommand { };
            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync("/api/identity/user", content);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AddUserRole_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var command = new AddUserRoleCommand { RoleName = "User", UserEmail = "test@test.com" };
            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/identity/userRole", content);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RemoveUserRole_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var command = new DeleteUserRoleCommand { RoleName = "User", UserEmail = "test@test.com" };
            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("/api/identity/userRole", UriKind.Relative),
                Content = content
            };

            // act
            var result = await client.SendAsync(request);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}