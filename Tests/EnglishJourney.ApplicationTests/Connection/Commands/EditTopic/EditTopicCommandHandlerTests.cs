using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.EditConnectionTopic.Tests
{
    [ExcludeFromCodeCoverage]
    public class EditTopicCommandHandlerTests
    {
        private EditTopicCommandHandler CreateEditTopicHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out ConnectionTopic topic, bool service = true)
        {
            topic = new ConnectionTopic
            {
                Id = 1,
                Topic = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var loggerMock = new Mock<ILogger<EditTopicCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new EditTopicCommandHandler(connectionRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
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

        [Fact]
        public async Task Handle_EditConnectionTopic_ShouldThrowForbidException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateEditTopicHandler(out var connectionRepositoryMock, out var topic, false);
            var command = new EditTopicCommand
            {
                Id = topic.Id,
                Topic = "Edit topic"
            };

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id)).ReturnsAsync(topic);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}