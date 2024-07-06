using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.CreateCategory.Tests
{
    public class CreateCategoryCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateCategory()
        {
            // arrange
            var command = new CreateCategoryCommand();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateCategoryCommandHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateCategoryCommandHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.CreateFlashcardCategory(It.IsAny<Domain.Entities.FlashcardCategory>()), Times.Once);
            flashcardRepositoryMock.Verify(f => f.CreateFlashcardBox(It.IsAny<Domain.Entities.FlashcardBox>()), Times.Exactly(6));
        }
    }
}