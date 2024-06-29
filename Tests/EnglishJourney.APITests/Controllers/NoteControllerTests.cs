using EnglishJourney.Application.Note.Commands.CreateNote;
using EnglishJourney.Application.Note.Commands.EditNote;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace EnglishJourney.API.Controllers.Tests
{
    public class NoteControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly Mock<INoteRepository> noteRepositoryMock = new();

        public NoteControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.Replace(ServiceDescriptor.Scoped(typeof(INoteRepository),
                                                _ => noteRepositoryMock.Object));
                });
            });
        }

        [Fact]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/notes");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllArchived_ForValidRequest_Returns200Ok()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/notes/archived");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Archive_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var note = new Domain.Entities.Note { Id = 1, Title = "Test" };
            noteRepositoryMock.Setup(m => m.GetById(note.Id)).ReturnsAsync(note);

            // act
            var result = await client.PatchAsync("/api/notes/1/archive", null);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Archive_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.PatchAsync("/api/notes/1/archive", null);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeArchive_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var note = new Domain.Entities.Note { Id = 1, Title = "Test" };
            noteRepositoryMock.Setup(m => m.GetById(note.Id)).ReturnsAsync(note);

            // act
            var result = await client.PatchAsync("/api/notes/1/dearchive", null);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeArchive_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.PatchAsync("/api/notes/1/dearchive", null);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var note = new Domain.Entities.Note { Id = 1, Title = "Test" };
            noteRepositoryMock.Setup(m => m.GetById(note.Id)).ReturnsAsync(note);

            var command = new EditNoteCommand { Title = "Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync("/api/notes/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Update_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new EditNoteCommand { Title = "Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PatchAsync("/api/notes/1", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForValidRequest_Returns204NoContent()
        {
            // arrange
            var client = factory.CreateClient();
            var note = new Domain.Entities.Note { Id = 1, Title = "Test" };
            noteRepositoryMock.Setup(m => m.GetById(note.Id)).ReturnsAsync(note);

            // act
            var result = await client.DeleteAsync("/api/notes/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForInvalidRequest_Returns404NotFound()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var result = await client.DeleteAsync("/api/notes/1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_ForValidRequest_Returns201Created()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new CreateNoteCommand { Title = "Test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/notes", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
            result.Headers.Location?.LocalPath.ToString().Should().Be("/api/notes");
        }

        [Fact]
        public async Task Create_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = factory.CreateClient();

            var command = new CreateNoteCommand();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var result = await client.PostAsync("/api/notes", jsonContent);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}