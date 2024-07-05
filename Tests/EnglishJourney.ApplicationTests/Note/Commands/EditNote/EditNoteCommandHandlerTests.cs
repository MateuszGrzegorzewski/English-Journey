﻿using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.EditNote.Tests
{
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

            return new EditNoteCommandHandler(noteRepositoryMock.Object, loggerMock.Object);
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
    }
}