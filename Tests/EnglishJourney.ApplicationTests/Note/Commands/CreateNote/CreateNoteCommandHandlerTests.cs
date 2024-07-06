using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.CreateNote.Tests
{
    [ExcludeFromCodeCoverage]
    public class CreateNoteCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateNote()
        {
            // arrange
            var command = new CreateNoteCommand();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateNoteCommandHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateNoteCommandHandler(noteRepositoryMock.Object, mapper, userContextMock.Object, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            //assert
            noteRepositoryMock.Verify(n => n.Create(It.IsAny<Domain.Entities.Note>()), Times.Once);
        }

        [Fact()]
        public async void Handle_CreateNote_ShouldThrownException_WhenUserDoesNotExist()
        {
            // arrange
            var command = new CreateNoteCommand();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateNoteCommandHandler>>();

            var userContextMock = new Mock<IUserContext>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateNoteCommandHandler(noteRepositoryMock.Object, mapper, userContextMock.Object, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact()]
        public async void Handle_CreateNote_ShouldThrownException_WhenNoAuthorized()
        {
            // arrange
            var command = new CreateNoteCommand();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateNoteCommandHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new CreateNoteCommandHandler(noteRepositoryMock.Object, mapper, userContextMock.Object, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}