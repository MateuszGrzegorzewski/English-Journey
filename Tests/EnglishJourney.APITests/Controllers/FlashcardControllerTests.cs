using EnglishJourney.APITests;
using EnglishJourney.Application.Flashcard;
using EnglishJourney.Application.Flashcard.Commands.CreateCategory;
using EnglishJourney.Application.Flashcard.Commands.CreateFlashcard;
using EnglishJourney.Application.Flashcard.Commands.EditCategory;
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
    public class FlashcardControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly Mock<IFlashcardRepository> flashcardRepositoryMock = new();

        public FlashcardControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.Replace(ServiceDescriptor.Scoped(typeof(IFlashcardRepository),
                                                _ => flashcardRepositoryMock.Object));
                });
            });
        }

        [Fact]
        public async void GetAllFlashcardsCategories_Returns200Ok()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/flashcards/category");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void GetCategoryById_ForValidRequest_Returns200Ok()
        {
            // arrange
            var id = 1;
            var flashcardCategory = new FlashcardCategory { Id = id, Name = "Test", UserId = "1" };
            flashcardRepositoryMock.Setup(m => m.GetFlashardCategoryById(id))
                .ReturnsAsync(flashcardCategory);
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/flashcards/category/{id}");
            var flashcardCategoryDto = await result.Content.ReadFromJsonAsync<FlashcardCategoryDto>();

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            flashcardCategoryDto?.Id.Should().Be(id);
            flashcardCategoryDto?.Name.Should().Be("Test");
        }

        [Fact]
        public async void GetCategoryById_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/flashcards/category/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void UpdateCategory_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var category = new FlashcardCategory { Name = "Test", UserId = "1" };
            flashcardRepositoryMock.Setup(m => m.GetFlashardCategoryById(1))
                .ReturnsAsync(category);
            var client = factory.CreateClient();

            var command = new EditCategoryCommand { Name = "Edited Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync($"/api/flashcards/category/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void UpdateCategory_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new EditCategoryCommand { Name = "Edited Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync($"/api/flashcards/category/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void DeleteCategory_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var category = new FlashcardCategory { Name = "Test", UserId = "1" };
            flashcardRepositoryMock.Setup(m => m.GetFlashardCategoryById(1))
                .ReturnsAsync(category);
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync($"/api/flashcards/category/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void DeleteCategory_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync($"/api/flashcards/category/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void CreateCategory_ForValidRequest_Returns201Created()
        {
            // arrange
            var client = factory.CreateClient();
            var command = new CreateCategoryCommand { Name = "Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/flashcards/category", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
            result.Headers.Location?.LocalPath.ToString().Should().Be("/api/flashcards/category/0");
        }

        [Fact]
        public async void CreateCategory_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = factory.CreateClient();
            var command = new CreateCategoryCommand();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/flashcards/category", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void GetBoxById_ForValidRequest_Returns200Ok()
        {
            // arrange
            var id = 1;
            var flashcardBox = new FlashcardBox { Id = id, BoxNumber = 2, FlashcardCategory = new FlashcardCategory { Name = "Test", UserId = "1" } };
            flashcardRepositoryMock.Setup(m => m.GetFlashardBoxById(id))
                .ReturnsAsync(flashcardBox);
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/flashcards/box/{id}");
            var flashcardBoxDto = await result.Content.ReadFromJsonAsync<FlashcardBoxDto>();

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            flashcardBoxDto?.Id.Should().Be(id);
            flashcardBoxDto?.BoxNumber.Should().Be(2);
        }

        [Fact]
        public async void GetBoxById_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/flashcards/box/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void DeleteFlashcard_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var flashcard = new Flashcard
            {
                Phrase = "Test",
                FlashcardBox = new FlashcardBox { Id = 1, FlashcardCategory = new FlashcardCategory { Name = "Test", UserId = "1" } }
            };
            var client = factory.CreateClient();
            flashcardRepositoryMock.Setup(m => m.GetFlashardById(1))
                .ReturnsAsync(flashcard);

            // act
            var result = await client.DeleteAsync("/api/flashcards/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void DeleteFlashcard_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync("/api/flashcards/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void CreateFlashcard_ForValidRequest_Returns201Created()
        {
            // arrange
            var client = factory.CreateClient();
            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(It.IsAny<int>()))
                .ReturnsAsync(new FlashcardBox { Id = 1, FlashcardCategory = new FlashcardCategory { Name = "Test", UserId = "1" } });
            var command = new CreateFlashcardCommand { Phrase = "Test", Definition = "Test definition" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/flashcards/box/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
            result.Headers.Location?.LocalPath.ToString().Should().Be("/api/flashcards/box/1");
        }

        [Fact]
        public async void CreateFlashcard_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = factory.CreateClient();
            var command = new CreateFlashcardCommand();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/flashcards/box/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void TestFlashcards_ForValidRequest_Returns200Ok()
        {
            // arrange
            var client = factory.CreateClient();
            var testResults = new Dictionary<int, bool> { { 1, true } };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(testResults), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync($"/api/flashcards/box/1/test?boxId=1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
            result.Headers.Location?.LocalPath.ToString().Should().Be("/api/flashcards/box/1");
        }

        [Fact]
        public async void TestFlashcards_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = factory.CreateClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync($"/api/flashcards/box/1/test?boxId=1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}