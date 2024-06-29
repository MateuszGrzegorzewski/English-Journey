using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.EditConnectionTopic.Tests
{
    public class EditTopicCommandHandlerTests
    {
        private EditTopicCommandHandler CreateEditTopicHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out Domain.Entities.ConnectionTopic topic)
        {
            topic = new Domain.Entities.ConnectionTopic
            {
                Id = 1,
                Topic = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var loggerMock = new Mock<ILogger<EditTopicCommandHandler>>();

            return new EditTopicCommandHandler(connectionRepositoryMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Handle_EditConnectionTopic_ShouldEditTopic_WhenTopicExists()
        {
            // arrange
            var handler = CreateEditTopicHandler(out var connectionRepositoryMock, out var topic);
            var command = new EditTopicCommand
            {
                Id = topic.Id,
                Topic = "Edit topic"
            };

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id)).ReturnsAsync(topic);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetTopicById(topic.Id), Times.Once());
            connectionRepositoryMock.Verify(c => c.Commit(), Times.Once());
            topic.Topic.Should().Be("Edit topic");
        }

        [Fact]
        public async Task Handle_EditConnectionTopic_ShouldThrowNotFoundException_WhenTopicDoesNotExist()
        {
            // arrange
            var handler = CreateEditTopicHandler(out var connectionRepositoryMock, out var topic);
            var command = new EditTopicCommand
            {
                Id = topic.Id,
                Topic = "Edit topic"
            };

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}