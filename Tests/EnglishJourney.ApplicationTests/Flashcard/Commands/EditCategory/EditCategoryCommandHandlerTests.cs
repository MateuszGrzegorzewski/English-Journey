using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.EditCategory.Tests
{
    [ExcludeFromCodeCoverage]
    public class EditCategoryCommandHandlerTests
    {
        private EditCategoryCommandHandler CreateEditCategoryHandler(out Mock<IFlashcardRepository> flashcardRepositoryMock, out FlashcardCategory category, bool service = true)
        {
            category = new FlashcardCategory
            {
                Id = 1,
                Name = "Test"
            };

            flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var loggerMock = new Mock<ILogger<EditCategoryCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new EditCategoryCommandHandler(flashcardRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_EditCategory_ShouldEditCategory_WhenCategoryExists()
        {
            // arrange
            var handler = CreateEditCategoryHandler(out var flashcardRepositoryMock, out var category);
            var command = new EditCategoryCommand
            {
                Id = category.Id,
                Name = "Changed Name"
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id)).ReturnsAsync(category);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardCategoryById(category.Id), Times.Once());
            flashcardRepositoryMock.Verify(f => f.Commit(), Times.Once);
            category.Name.Should().Be("Changed Name");
        }

        [Fact]
        public async Task Handle_EditCategory_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            // arrange
            var handler = CreateEditCategoryHandler(out var flashcardRepositoryMock, out var category);
            var command = new EditCategoryCommand
            {
                Id = category.Id,
                Name = "Changed Name"
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_EditCategory_ShouldThrowForbidException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateEditCategoryHandler(out var flashcardRepositoryMock, out var category, false);
            var command = new EditCategoryCommand
            {
                Id = category.Id,
                Name = "Changed Name"
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id)).ReturnsAsync(category);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}