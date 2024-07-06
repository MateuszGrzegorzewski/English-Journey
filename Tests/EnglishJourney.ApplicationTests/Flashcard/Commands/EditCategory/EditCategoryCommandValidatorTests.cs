using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.EditCategory.Tests
{
    [ExcludeFromCodeCoverage]
    public class EditCategoryCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new EditCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object);
            var command = new EditCategoryCommand()
            {
                Name = "name",
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

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new EditCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object);
            var command = new EditCategoryCommand()
            {
                Name = "Test category which have too much characters",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(f => f.Name);
        }

        [Fact]
        public void Validate_WithDuplicateCategoryName_ShouldHaveValidationError()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            flashcardRepositoryMock.Setup(repo => repo.GetFlashcardCategoryByName(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Domain.Entities.FlashcardCategory()
                {
                    Name = "Existing Category",
                });

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new EditCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object);
            var command = new EditCategoryCommand()
            {
                Name = "Existing Category",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(f => f.Name)
                  .WithErrorMessage("Existing Category is not unique name for category");
        }

        [Fact()]
        public void Validate_WithValidCommandAndNullUser_ShouldThrowException()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var userContextMock = new Mock<IUserContext>();

            // act & assert
            Assert.Throws<UnauthorizedAccessException>(() => new EditCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object));
        }
    }
}