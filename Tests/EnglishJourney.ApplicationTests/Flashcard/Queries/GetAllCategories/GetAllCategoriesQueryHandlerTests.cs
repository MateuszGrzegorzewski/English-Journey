using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Queries.GetAllCategories.Tests
{
    [ExcludeFromCodeCoverage]
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

        [Fact()]
        public async void Handle_GetAllCategories_ShouldThrowException_WhenUserIsNull()
        {
            // arrange
            var query = new GetAllCategoriesQuery();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllCategoriesQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllCategoriesQueryHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact()]
        public async void Handle_GetAllCategories_ShouldThrowException_WhenNoAuthorized()
        {
            // arrange
            var query = new GetAllCategoriesQuery();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            flashcardRepositoryMock.Setup(f => f.GetAllFlashcardCategories(It.IsAny<string>())).ReturnsAsync(new List<FlashcardCategory> { new FlashcardCategory { Name = "Test name" } });

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllCategoriesQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new GetAllCategoriesQueryHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}