using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.DeleteFlashcard.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeleteFlashcardCommandHandlerTests
    {
        private DeleteFlashcardCommandHandler CreateDeleteFlashcardHandler(out Mock<IFlashcardRepository> flashcardRepositoryMock, out Domain.Entities.Flashcard flashcard, bool service = true)
        {
            flashcard = new Domain.Entities.Flashcard
            {
                Id = 1,
                Phrase = "Test",
                FlashcardBox = new FlashcardBox
                {
                    Id = 1,
                    FlashcardCategory = new FlashcardCategory
                    {
                        Id = 1,
                        Name = "Test"
                    }
                }
            };

            flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var loggerMock = new Mock<ILogger<DeleteFlashcardCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new DeleteFlashcardCommandHandler(flashcardRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteFlashcard_ShouldDeleteFlashcard_WhenFlashcardExists()
        {
            // arrange
            var handler = CreateDeleteFlashcardHandler(out var flashcardRepositoryMock, out var flashcard);
            var command = new DeleteFlashcardCommand
            {
                Id = flashcard.Id
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardById(flashcard.Id)).ReturnsAsync(flashcard);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardById(flashcard.Id), Times.Once);
            flashcardRepositoryMock.Verify(f => f.DeleteFlashcard(It.Is<Domain.Entities.Flashcard>(c => c.Id == flashcard.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteFlashcard_ShouldThrowNotFoundException_WhenFlashcardDoesNotExist()
        {
            // arrange
            var handler = CreateDeleteFlashcardHandler(out var flashcardRepositoryMock, out var flashcard);
            var command = new DeleteFlashcardCommand
            {
                Id = flashcard.Id
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardById(flashcard.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeleteFlashcard_ShouldThrowForbidException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateDeleteFlashcardHandler(out var flashcardRepositoryMock, out var flashcard, false);

            var command = new DeleteFlashcardCommand
            {
                Id = flashcard.Id
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardById(flashcard.Id)).ReturnsAsync(flashcard);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}