using EnglishJourney.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.CreateFlashcard.Tests
{
    [ExcludeFromCodeCoverage]
    public class CreateFlashcardCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var validator = new CreateFlashcardCommandValidator(flashcardRepositoryMock.Object);
            var command = new CreateFlashcardCommand()
            {
                Phrase = "name",
                Definition = "definition"
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInValidCommand_ShouldHaveValidationError()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var validator = new CreateFlashcardCommandValidator(flashcardRepositoryMock.Object);
            var command = new CreateFlashcardCommand()
            {
                Phrase = "T",
                Definition = "D"
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(f => f.Phrase);
            result.ShouldHaveValidationErrorFor(f => f.Definition);
        }
    }
}