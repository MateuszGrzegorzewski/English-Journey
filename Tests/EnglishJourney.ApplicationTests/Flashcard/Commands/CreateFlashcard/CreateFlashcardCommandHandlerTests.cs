using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.CreateFlashcard.Tests
{
    [ExcludeFromCodeCoverage]
    public class CreateFlashcardCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateFlashcard()
        {
            // arrange
            var command = new CreateFlashcardCommand()
            {
                FlashcardBoxId = 1
            };

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(It.IsAny<int>())).ReturnsAsync(new FlashcardBox());

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateFlashcardCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateFlashcardCommandHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.CreateFlashcard(It.IsAny<Domain.Entities.Flashcard>()), Times.Once);
        }

        [Fact()]
        public async void Handle_CreateFlashcard_ShouldThrowException_WhenRequestDoesNotHaveBoxId()
        {
            // arrange
            var command = new CreateFlashcardCommand();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(It.IsAny<int>())).ReturnsAsync(new FlashcardBox());

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateFlashcardCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateFlashcardCommandHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact()]
        public async void Handle_CreateFlashcard_ShouldThrowException_WhenBoxIdDoesNotExist()
        {
            // arrange
            var command = new CreateFlashcardCommand()
            {
                FlashcardBoxId = 1
            };

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateFlashcardCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateFlashcardCommandHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact()]
        public async void Handle_CreateFlashcard_ShouldThrowException_WhenNoAuthorized()
        {
            // arrange
            var command = new CreateFlashcardCommand()
            {
                FlashcardBoxId = 1
            };

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(It.IsAny<int>())).ReturnsAsync(new FlashcardBox());

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateFlashcardCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new CreateFlashcardCommandHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}