using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.DeleteNote.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeleteNoteCommandHandlerTests
    {
        private DeleteNoteCommandHandler CreateDeleteNoteHandler(out Mock<INoteRepository> noteRepositoryMock, out Domain.Entities.Note note, bool service = true)
        {
            note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test"
            };

            noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<DeleteNoteCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new DeleteNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteNote_ShouldDeleteNote_WhenNoteExists()
        {
            // arrange
            var handler = CreateDeleteNoteHandler(out var noteRepositoryMock, out var note);
            var command = new DeleteNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.Delete(It.Is<Domain.Entities.Note>(n => n.Id == note.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteNote_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            // arrange
            var handler = CreateDeleteNoteHandler(out var noteRepositoryMock, out var note);
            var command = new DeleteNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeleteNote_ShouldThrownForbidException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateDeleteNoteHandler(out var noteRepositoryMock, out var note, false);
            var command = new DeleteNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}