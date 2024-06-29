using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.DeleteNote.Tests
{
    public class DeleteNoteCommandHandlerTests
    {
        private DeleteNoteCommandHandler CreateDeleteNoteHandler(out Mock<INoteRepository> noteRepositoryMock, out Domain.Entities.Note note)
        {
            note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test"
            };

            noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<DeleteNoteCommandHandler>>();

            return new DeleteNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object);
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
    }
}