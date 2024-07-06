using FluentValidation.TestHelper;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.EditNote.Tests
{
    [ExcludeFromCodeCoverage]
    public class EditNoteCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveAnyError()
        {
            // arrange
            var validator = new EditNoteCommandValidator();
            var command = new EditNoteCommand()
            {
                Title = "Test",
                Description = "Description",
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
            var validator = new EditNoteCommandValidator();
            var command = new EditNoteCommand()
            {
                Title = "T",
                Description = "Description",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(n => n.Title);
        }
    }
}