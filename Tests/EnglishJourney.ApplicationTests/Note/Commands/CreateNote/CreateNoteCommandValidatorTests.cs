using FluentValidation.TestHelper;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.CreateNote.Tests
{
    public class CreateNoteCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var validator = new CreateNoteCommandValidator();
            var command = new CreateNoteCommand()
            {
                Title = "Test",
                Description = "Description Test",
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
            var validator = new CreateNoteCommandValidator();
            var command = new CreateNoteCommand()
            {
                Title = "T",
                Description = "Description Test",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(n => n.Title);
        }
    }
}