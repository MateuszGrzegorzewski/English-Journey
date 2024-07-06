using EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute;
using FluentValidation.TestHelper;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.CreateAttribute.Tests
{
    [ExcludeFromCodeCoverage]
    public class CreateAttributeCommandValidationTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            //arrange
            var validator = new CreateAttributeCommandValidation();
            var command = new CreateAttributeCommand()
            {
                Word = "Test word",
                Definition = "Test definition"
            };

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInValidCommand_ShouldHaveValidationError()
        {
            //arrange
            var validator = new CreateAttributeCommandValidation();
            var command = new CreateAttributeCommand()
            {
                Word = "Test word which have too much characters",
                Definition = "Test definition"
            };

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(c => c.Word);
        }
    }
}