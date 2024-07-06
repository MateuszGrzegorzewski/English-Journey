using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.ArchiveNote.Tests
{
    public class ArchiveNoteCommandHandlerTests
    {
        private ArchiveNoteCommandHandler CreateArchiveNoteHandler(out Mock<INoteRepository> noteRepositoryMock, out Domain.Entities.Note note)
        {
            note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test",
                IsArchivized = false
            };

            noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<ArchiveNoteCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            return new ArchiveNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ArchiveNote_ShouldArchiveNote_WhenNoteExists()
        {
            // arrange
            var handler = CreateArchiveNoteHandler(out var noteRepositoryMock, out var note);
            var command = new ArchiveNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetById(note.Id), Times.Once);
            noteRepositoryMock.Verify(n => n.Commit(), Times.Once);
            note.IsArchivized.Should().Be(true);
        }

        [Fact]
        public async Task Handle_ArchiveNote_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            // arrange
            var handler = CreateArchiveNoteHandler(out var noteRepositoryMock, out var note);
            var command = new ArchiveNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}