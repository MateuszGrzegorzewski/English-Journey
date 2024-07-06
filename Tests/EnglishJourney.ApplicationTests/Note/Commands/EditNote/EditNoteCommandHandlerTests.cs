using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.EditNote.Tests
{
    [ExcludeFromCodeCoverage]
    public class EditNoteCommandHandlerTests
    {
        private EditNoteCommandHandler CreateEditNoteHandler(out Mock<INoteRepository> noteRepositoryMock, out Domain.Entities.Note note)
        {
            note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test",
                IsArchivized = false
            };

            noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<EditNoteCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            return new EditNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_EditNote_ShouldUpdateNote_WhenNoteExists()
        {
            // arrange
            var handler = CreateEditNoteHandler(out var noteRepositoryMock, out var note);
            var command = new EditNoteCommand
            {
                Id = note.Id,
                Title = "Edit title"
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetById(note.Id), Times.Once);
            noteRepositoryMock.Verify(n => n.Commit(), Times.Once);
            note.Title.Should().Be("Edit title");
        }

        [Fact]
        public async Task Handle_EditNote_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            // arrange
            var handler = CreateEditNoteHandler(out var noteRepositoryMock, out var note);
            var command = new EditNoteCommand
            {
                Id = note.Id,
                Title = "Edit title"
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_EditNote_ShouldThrownException_WhenNoAuthorized()
        {
            // arrange
            var note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test",
                IsArchivized = false
            };

            var noteRepositoryMock = new Mock<INoteRepository>();

            var loggerMock = new Mock<ILogger<EditNoteCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new EditNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);

            var command = new EditNoteCommand
            {
                Id = note.Id,
                Title = "Edit title"
            };

            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}