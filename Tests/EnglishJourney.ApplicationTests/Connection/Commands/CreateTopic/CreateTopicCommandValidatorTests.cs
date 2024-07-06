using EnglishJourney.Application.Connection.Commands.CreateConnectionTopic;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.CreateTopic.Tests
{
    public class CreateTopicCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            //arrange
            var connectionRepository = new Mock<IConnectionRepository>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", []);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new CreateTopicCommandValidator(connectionRepository.Object, userContextMock.Object);
            var command = new CreateTopicCommand()
            {
                Topic = "Test Topic",
            };

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInValidCommandAndExistingTopic_ShouldHaveValidationError()
        {
            // arrange
            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", []);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var connectionRepository = new Mock<IConnectionRepository>();
            connectionRepository.Setup(repo => repo.GetTopicsByName(It.IsAny<string>(), It.IsAny<string>()))
                                .ReturnsAsync(new ConnectionTopic()
                                {
                                    Topic = "Test Topic"
                                });

            var validator = new CreateTopicCommandValidator(connectionRepository.Object, userContextMock.Object);
            var command = new CreateTopicCommand()
            {
                Topic = "Test Topic",
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Topic)
                  .WithErrorMessage("Test Topic is not unique name for topic");
        }

        [Fact()]
        public void Validate_WithInValidCommand_ShouldHaveValidationError()
        {
            //arrange
            var connectionRepository = new Mock<IConnectionRepository>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", []);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var validator = new CreateTopicCommandValidator(connectionRepository.Object, userContextMock.Object);
            var command = new CreateTopicCommand()
            {
                Topic = "Test topic which have too much characters",
            };

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(c => c.Topic)
                  .WithErrorMessage("Too much characters");
        }
    }
}