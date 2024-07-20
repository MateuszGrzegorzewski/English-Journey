using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Domain.Statistic;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using Xunit;

namespace EnglishJourney.APITests.Controllers
{
    public class StatisticControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly Mock<IUserStatisticRepository> repositoryMock = new();

        public StatisticControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.Replace(ServiceDescriptor.Scoped(typeof(IUserStatisticRepository),
                                                                       _ => repositoryMock.Object));
                });
            });
        }

        [Fact]
        public async Task GetAllUserStatistics_ForValidRequest_ReturnsOk()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/statistics/users");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDemography_ForValidRequest_ReturnsOk()
        {
            // arrange
            repositoryMock.Setup(x => x.GetDemography()).ReturnsAsync(new List<DemographyResult>());
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/statistics/demography");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDemography_ForNullRequest_ReturnsNoContent()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/statistics/demography");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}