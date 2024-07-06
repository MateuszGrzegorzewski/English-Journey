using EnglishJourney.Application.Note.Commands.ArchiveNote;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.DeArchiveNote.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeArchiveNoteCommandHandlerTests
    {
        private DeArchiveNoteCommandHandler CreateDeArchiveNoteHandler(out Mock<INoteRepository> noteRepositoryMock, out Domain.Entities.Note note)
        {
            note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test",
                IsArchivized = true
            };

            noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<DeArchiveNoteCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            return new DeArchiveNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DeArchiveNote_ShouldDeArchiveNote_WhenNoteExists()
        {
            // arrange
            var handler = CreateDeArchiveNoteHandler(out var noteRepositoryMock, out var note);
            var command = new DeArchiveNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetById(note.Id), Times.Once);
            noteRepositoryMock.Verify(n => n.Commit(), Times.Once);
            note.IsArchivized.Should().Be(false);
        }

        [Fact]
        public async Task Handle_DeArchiveNote_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            // arrange
            var handler = CreateDeArchiveNoteHandler(out var noteRepositoryMock, out var note);
            var command = new DeArchiveNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeArchiveNote_ShouldThrowForbidException_WhenNoAuthorization()
        {
            // arrange
            var note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test",
                IsArchivized = false
            };

            var noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<DeArchiveNoteCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new DeArchiveNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);

            var command = new DeArchiveNoteCommand
            {
                Id = note.Id
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act && assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}