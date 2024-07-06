using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.DeleteCategory.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeleteCategoryCommandHandlerTests
    {
        private DeleteCategoryCommandHandler CreateDeleteCategoryHandler(out Mock<IFlashcardRepository> flashcardRepositoryMock, out FlashcardCategory category, bool service = true)
        {
            category = new FlashcardCategory
            {
                Id = 1,
                Name = "Test"
            };

            flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var loggerMock = new Mock<ILogger<DeleteCategoryCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new DeleteCategoryCommandHandler(flashcardRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteCategory_ShouldDeleteCategory_WhenCategoryExists()
        {
            // arrange
            var handler = CreateDeleteCategoryHandler(out var flashcardRepositoryMock, out var category);
            var command = new DeleteCategoryCommand
            {
                Id = category.Id
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id)).ReturnsAsync(category);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardCategoryById(category.Id), Times.Once);
            flashcardRepositoryMock.Verify(f => f.DeleteFlashcardCategory(It.Is<FlashcardCategory>(c => c.Id == category.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteCategory_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            // arrange
            var handler = CreateDeleteCategoryHandler(out var flashcardRepositoryMock, out var category);
            var command = new DeleteCategoryCommand
            {
                Id = category.Id
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeleteCategory_ShouldThrowException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateDeleteCategoryHandler(out var flashcardRepositoryMock, out var category, false);
            var command = new DeleteCategoryCommand
            {
                Id = category.Id
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id)).ReturnsAsync(category);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}