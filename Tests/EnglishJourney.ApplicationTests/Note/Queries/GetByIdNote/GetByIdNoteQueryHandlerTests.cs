using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Queries.GetByIdNote.Tests
{
    public class GetByIdNoteQueryHandlerTests
    {
        private GetByIdNoteQueryHandler CreateHandler(out Mock<INoteRepository> noteRepositoryMock, out Domain.Entities.Note note)
        {
            note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test"
            };

            noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetByIdNoteQueryHandler>>();

            return new GetByIdNoteQueryHandler(noteRepositoryMock.Object, mapper, loggerMock.Object);
        }

        [Fact]
        public async Task GetByIdNoteQueryHandler_ShouldReturnNote_WhenNoteExists()
        {
            // arrange
            var handler = CreateHandler(out var noteRepositoryMock, out var note);
            var query = new GetByIdNoteQuery(note.Id);
            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetById(note.Id), Times.Once);
        }

        [Fact]
        public async Task GetByIdNoteQueryHandler_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            // arrange
            var handler = CreateHandler(out var noteRepositoryMock, out var note);
            var query = new GetByIdNoteQuery(note.Id);
            noteRepositoryMock.Setup(n => n.GetById(note.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}