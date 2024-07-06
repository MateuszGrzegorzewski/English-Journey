using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Commands.CreateCategory.Tests
{
    public class CreateCategoryCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new CreateCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object);
            var command = new CreateCategoryCommand()
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

            var validator = new CreateCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object);
            var command = new CreateCategoryCommand()
            {
                Name = "Test category which have too much characters",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(f => f.Name)
               .WithErrorMessage("Too much characters");
        }

        [Fact]
        public void Validate_WithDuplicateCategoryName_ShouldHaveValidationError()
        {
            // arrange
            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();
            flashcardRepositoryMock.Setup(repo => repo.GetFlashcardCategoryByName(It.IsAny<string>(), It.IsAny<string>())).
                ReturnsAsync(new Domain.Entities.FlashcardCategory()
                {
                    Name = "Existing Category",
                });

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new CreateCategoryCommandValidator(flashcardRepositoryMock.Object, userContextMock.Object);
            var command = new CreateCategoryCommand()
            {
                Name = "Existing Category",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(f => f.Name)
                  .WithErrorMessage("Existing Category is not unique name for category");
        }
    }
}