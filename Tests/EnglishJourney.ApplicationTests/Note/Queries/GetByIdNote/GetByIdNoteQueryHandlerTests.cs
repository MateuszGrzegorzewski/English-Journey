using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Note.Queries.GetByIdNote.Tests
{
    [ExcludeFromCodeCoverage]
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

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            return new GetByIdNoteQueryHandler(noteRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);
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

        [Fact]
        public async Task GetByIdNoteQueryHandler_ShouldThrownForbidException_WhenNoAuthorized()
        {
            // arrange
            var note = new Domain.Entities.Note
            {
                Id = 1,
                Title = "Test"
            };

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetByIdNoteQueryHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new GetByIdNoteQueryHandler(noteRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            var query = new GetByIdNoteQuery(note.Id);
            noteRepositoryMock.Setup(n => n.GetById(note.Id)).ReturnsAsync(note);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}