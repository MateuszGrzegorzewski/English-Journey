using AutoMapper;
using EnglishJourney.Application.Mappings;
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

            var handler = new CreateCategoryCommandHandler(flashcardRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.CreateFlashcardCategory(It.IsAny<Domain.Entities.FlashcardCategory>()), Times.Once);
            flashcardRepositoryMock.Verify(f => f.CreateFlashcardBox(It.IsAny<Domain.Entities.FlashcardBox>()), Times.Exactly(6));
        }
    }
}