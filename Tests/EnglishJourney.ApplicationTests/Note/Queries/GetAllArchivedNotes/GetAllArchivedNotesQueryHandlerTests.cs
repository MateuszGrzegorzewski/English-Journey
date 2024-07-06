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

namespace EnglishJourney.Application.Note.Queries.GetAllArchivedNotes.Tests
{
    [ExcludeFromCodeCoverage]
    public class GetAllArchivedNotesQueryHandlerTests
    {
        [Fact()]
        public async void Handle_GetArchivedNotes()
        {
            // arrange
            var query = new GetAllArchivedNotesQuery();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllArchivedNotesQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllArchivedNotesQueryHandler(noteRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetAllArchived(currentUser.Id), Times.Once);
        }

        [Fact()]
        public async void Handle_GetArchivedNotes_ShouldThrownException_WhenUserIsNull()
        {
            // arrange
            var query = new GetAllArchivedNotesQuery();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllArchivedNotesQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllArchivedNotesQueryHandler(noteRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact()]
        public async void Handle_GetArchivedNotes_ShouldThrownForbidException_WhenNoAuthorized()
        {
            // arrange
            var query = new GetAllArchivedNotesQuery();

            var noteRepositoryMock = new Mock<INoteRepository>();
            noteRepositoryMock.Setup(n => n.GetAllArchived(It.IsAny<string>()))
                .ReturnsAsync(new List<Domain.Entities.Note> { new Domain.Entities.Note() { Title = "Test" } });

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllArchivedNotesQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new GetAllArchivedNotesQueryHandler(noteRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}