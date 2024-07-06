using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.TestFlashcards.Tests
{
    public class TestFlashcardsCommandHandlerTests
    {
        private Mock<IFlashcardRepository> flashcardRepositoryMock;
        private Mock<ILogger<TestFlashcardsCommandHandler>> loggerMock;
        private TestFlashcardsCommandHandler handler;

        private Domain.Entities.Flashcard flashcard;
        private FlashcardBox box;
        private FlashcardBox nextBox;

        public TestFlashcardsCommandHandlerTests()
        {
            flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            loggerMock = new Mock<ILogger<TestFlashcardsCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            handler = new TestFlashcardsCommandHandler(flashcardRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);

            flashcard = new Domain.Entities.Flashcard()
            {
                Id = 1,
                Phrase = "Test Phrase",
                Definition = "Test Definition",
                FlashcardBoxId = 10,
            };

            box = new FlashcardBox()
            {
                Id = 10,
                FlashcardCategoryId = 1,
                BoxNumber = 1
            };

            nextBox = new FlashcardBox()
            {
                Id = 11,
                FlashcardCategoryId = 1,
                BoxNumber = 2
            };
        }

        [Fact]
        public async Task Handle_TestFlashcards_ShouldNotHaveValidationError()
        {
            // arrange
            var command = new TestFlashcardsCommand()
            {
                TestResults = new Dictionary<int, bool>()
            {
                { flashcard.Id, true }
            }
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardById(flashcard.Id)).ReturnsAsync(flashcard);
            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(flashcard.FlashcardBoxId)).ReturnsAsync(box);
            flashcardRepositoryMock.Setup(f => f.GetFlashcardBoxByCategoryIdAndBoxNumber(box.FlashcardCategoryId, box.BoxNumber + 1)).ReturnsAsync(nextBox);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardById(flashcard.Id), Times.Once());
            flashcardRepositoryMock.Verify(f => f.GetFlashardBoxById(box.Id), Times.Once);
            flashcardRepositoryMock.Verify(f => f.GetFlashcardBoxByCategoryIdAndBoxNumber(box.FlashcardCategoryId, box.BoxNumber + 1), Times.Once);
            Assert.Equal(11, flashcard.FlashcardBoxId);
        }

        [Fact]
        public async Task Handle_WhenTestResultsAreNull_ShouldReturnError()
        {
            // arrange
            var command = new TestFlashcardsCommand();

            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_TestFlashcardsWithNullFlashcardValue()
        {
            // arrange
            var command = new TestFlashcardsCommand()
            {
                TestResults = new Dictionary<int, bool>()
            {
                { flashcard.Id, true }
            }
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardById(flashcard.Id)).ReturnsAsync((Domain.Entities.Flashcard)null);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardById(flashcard.Id), Times.Once());
            flashcardRepositoryMock.Verify(f => f.GetFlashardBoxById(box.Id), Times.Never);
            flashcardRepositoryMock.Verify(f => f.GetFlashcardBoxByCategoryIdAndBoxNumber(box.FlashcardCategoryId, box.BoxNumber + 1), Times.Never);
            Assert.Equal(10, flashcard.FlashcardBoxId);
        }

        [Fact]
        public async Task Handle_WhenBoxDoesNotExist_ShouldReturnError()
        {
            // arrange
            var command = new TestFlashcardsCommand()
            {
                TestResults = new Dictionary<int, bool>()
            {
                { flashcard.Id, true }
            }
            };

            flashcardRepositoryMock.Setup(f => f.GetFlashardById(flashcard.Id)).ReturnsAsync(flashcard);
            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(flashcard.FlashcardBoxId)).ReturnsAsync((FlashcardBox)null);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}