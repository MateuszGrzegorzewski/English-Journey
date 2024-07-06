using EnglishJourney.Application.Connection.Commands.EditConnectionTopic;
using EnglishJourney.Application.Connection.Commands.EditTopic;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.ApplicationTests.Connection.Commands.EditTopic
{
    [ExcludeFromCodeCoverage]
    public class EditTopicCommandValidatorTests
    {
        [Fact]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var connectionRepository = new Mock<IConnectionRepository>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new EditTopicCommandValidator(connectionRepository.Object, userContextMock.Object);
            var command = new EditTopicCommand()
            {
                Topic = "Valid Topic",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithInValidCommandAndExistingTopic_ShouldHaveValidationError()
        {
            // arrange
            var connectionRepository = new Mock<IConnectionRepository>();
            connectionRepository.Setup(repo => repo.GetTopicsByName(It.IsAny<string>(), It.IsAny<string>()))
                                .ReturnsAsync(new ConnectionTopic()
                                {
                                    Topic = "Existing Topic"
                                });

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new EditTopicCommandValidator(connectionRepository.Object, userContextMock.Object);
            var command = new EditTopicCommand()
            {
                Topic = "Existing Topic",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Topic)
                  .WithErrorMessage("Existing Topic is not unique name for topic");
        }

        [Fact]
        public void Validate_WithInValidCommand_ShouldHaveValidationError()
        {
            // arrange
            var connectionRepository = new Mock<IConnectionRepository>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new EditTopicCommandValidator(connectionRepository.Object, userContextMock.Object);
            var command = new EditTopicCommand()
            {
                Topic = "Topic with too many characters exceeding the limit",
            };

            // act
            var result = validator.TestValidate(command);

            //aAssert
            result.ShouldHaveValidationErrorFor(c => c.Topic)
                  .WithErrorMessage("Too much characters");
        }

        [Fact]
        public void Validate_WithValidCommandAndNullUser_ShouldThrowException()
        {
            // arrange
            var connectionRepository = new Mock<IConnectionRepository>();

            var userContextMock = new Mock<IUserContext>();

            // act & assert
            Assert.Throws<UnauthorizedAccessException>(() => new EditTopicCommandValidator(connectionRepository.Object, userContextMock.Object));
        }
    }
}