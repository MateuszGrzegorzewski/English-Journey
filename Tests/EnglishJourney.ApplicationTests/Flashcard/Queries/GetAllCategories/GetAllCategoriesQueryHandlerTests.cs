using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Queries.GetAllCategories.Tests
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact()]
        public async void Handle_GetAllCategories()
        {
            // arrange
            var query = new GetAllCategoriesQuery();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllCategoriesQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllCategoriesQueryHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetAllFlashcardCategories(currentUser.Id), Times.Once);
        }
    }
}