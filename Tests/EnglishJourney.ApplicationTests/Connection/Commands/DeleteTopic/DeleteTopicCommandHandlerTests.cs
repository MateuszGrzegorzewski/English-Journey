using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.DeleteTopic.Tests
{
    public class DeleteTopicCommandHandlerTests
    {
        private DeleteTopicCommandHandler CreateDeleteTopicHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out Domain.Entities.ConnectionTopic topic)
        {
            topic = new Domain.Entities.ConnectionTopic
            {
                Id = 1,
                Topic = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var loggerMock = new Mock<ILogger<DeleteTopicCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(true);

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
    }
}