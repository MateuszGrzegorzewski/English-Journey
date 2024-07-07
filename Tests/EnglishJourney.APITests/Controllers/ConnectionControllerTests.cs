using EnglishJourney.APITests;
using EnglishJourney.Application.Connection;
using EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute;
using EnglishJourney.Application.Connection.Commands.CreateConnectionTopic;
using EnglishJourney.Application.Connection.Commands.EditConnectionTopic;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
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

namespace EnglishJourney.API.Controllers.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConnectionControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly Mock<IConnectionRepository> connectionRepositoryMock = new();

        public ConnectionControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.Replace(ServiceDescriptor.Scoped(typeof(IConnectionRepository),
                                                _ => connectionRepositoryMock.Object));
                });
            });
        }

        [Fact]
        public async void GetAll_Returns200Ok()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/connections");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void GetById_ForValidRequest_Returns200Ok()
        {
            // arrange
            var id = 1;
            var connectionTopic = new ConnectionTopic { Id = id, Topic = "Test", UserId = "1" };
            connectionRepositoryMock.Setup(c => c.GetTopicById(id)).ReturnsAsync(connectionTopic);
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/connections/{id}");
            var connectionTopicDto = await result.Content.ReadFromJsonAsync<ConnectionTopicDto>();

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            connectionTopicDto.Should().NotBeNull();
            connectionTopicDto!.Id.Should().Be(id);
            connectionTopicDto!.Topic.Should().Be("Test");
        }

        [Fact]
        public async void GetById_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var id = 123;
            connectionRepositoryMock.Setup(c => c.GetTopicById(id)).ReturnsAsync((ConnectionTopic?)null);
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/connections/{id}");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void UpdateTopic_WithValidRequest_Returns204NoContent()
        {
            // arrange
            var connectionTopic = new ConnectionTopic { Topic = "Test", UserId = "1" };
            connectionRepositoryMock.Setup(c => c.GetTopicById(1)).ReturnsAsync(connectionTopic);
            var client = factory.CreateClient();

            var command = new EditTopicCommand { Topic = "Edited Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync("/api/connections/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void UpdateTopic_WithInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new EditTopicCommand { Id = 1, Topic = "Edited Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync("/api/connections/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void DeleteTopic_WithValidRequest_Returns204NoContent()
        {
            // arrange
            var connectionTopic = new ConnectionTopic { Topic = "Test", UserId = "1" };
            connectionRepositoryMock.Setup(c => c.GetTopicById(1)).ReturnsAsync(connectionTopic);
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync("/api/connections/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void DeleteTopic_WithInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync("/api/connections/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAttribute_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var connectionAttribute = new ConnectionAttribute { Word = "Test", Topic = new ConnectionTopic { Topic = "Test", UserId = "1" } };
            connectionRepositoryMock.Setup(c => c.GetAttributeById(1)).ReturnsAsync(connectionAttribute);
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync("/api/connections/attributes/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAttribute_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync("/api/connections/attributes/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void CreateTopic_ForValidRequest_Returns201Created()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new CreateTopicCommand { Topic = "Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/connections", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
            result.Headers.Location?.LocalPath.ToString().Should().Be("/api/connections/0");
        }

        [Fact]
        public async void CreateTopic_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new CreateTopicCommand();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/connections", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void CreateAttribute_ForValidRequest_Returns201Created()
        {
            // arrange
            var connectionTopic = new ConnectionTopic { Topic = "Test", UserId = "1" };
            connectionRepositoryMock.Setup(c => c.GetTopicById(1)).ReturnsAsync(connectionTopic);
            var client = factory.CreateClient();

            var command = new CreateAttributeCommand { Word = "Test", Definition = "Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/connections/1/attributes", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
            result.Headers.Location?.LocalPath.ToString().Should().Be("/api/connections/1");
        }

        [Fact]
        public async void CreateAttribute_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new CreateAttributeCommand();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/connections/1/attributes", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}