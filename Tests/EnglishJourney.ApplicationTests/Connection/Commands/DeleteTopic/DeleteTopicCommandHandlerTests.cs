using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.DeleteTopic.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeleteTopicCommandHandlerTests
    {
        private DeleteTopicCommandHandler CreateDeleteTopicHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out ConnectionTopic topic, bool service = true)
        {
            topic = new ConnectionTopic
            {
                Id = 1,
                Topic = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var loggerMock = new Mock<ILogger<DeleteTopicCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new DeleteTopicCommandHandler(connectionRepositoryMock.Object, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteTopic_ShouldDeleteTopic_WhenTopicExists()
        {
            // arrange
            var handler = CreateDeleteTopicHandler(out var connectionRepositoryMock, out var topic);
            var command = new DeleteTopicCommand
            {
                Id = topic.Id
            };

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id)).ReturnsAsync(topic);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetTopicById(topic.Id), Times.Once);
            connectionRepositoryMock.Verify(c => c.DeleteTopic(It.Is<Domain.Entities.ConnectionTopic>(t => t.Id == topic.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteTopic_ShouldThrowNotFoundException_WhenTopicDoesNotExist()
        {
            // arrange
            var handler = CreateDeleteTopicHandler(out var connectionRepositoryMock, out var topic);
            var command = new DeleteTopicCommand
            {
                Id = topic.Id
            };

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeleteTopic_ShouldThrowForbidException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateDeleteTopicHandler(out var connectionRepositoryMock, out var topic, false);
            var command = new DeleteTopicCommand
            {
                Id = topic.Id
            };

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id)).ReturnsAsync(topic);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}