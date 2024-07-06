using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Queries.GetAllArchivedNotes.Tests
{
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
            var currentUser = new CurrentUser("user-id", "test@test.com", []);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeNotes(It.IsAny<Domain.Entities.Note>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllArchivedNotesQueryHandler(noteRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetAllArchived(currentUser.Id), Times.Once);
        }
    }
}