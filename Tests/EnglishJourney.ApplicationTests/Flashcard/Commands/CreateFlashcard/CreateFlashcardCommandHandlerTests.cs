using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.CreateFlashcard.Tests
{
    public class CreateFlashcardCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateFlashcard()
        {
            // arrange
            var command = new CreateFlashcardCommand();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateFlashcardCommandHandler>>();

            var handler = new CreateFlashcardCommandHandler(flashcardRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.CreateFlashcard(It.IsAny<Domain.Entities.Flashcard>()), Times.Once);
        }
    }
}