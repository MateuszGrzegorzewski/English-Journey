using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Queries.GetByTopicId.Tests
{
    public class GetTopicByIdQueryHandlerTests
    {
        private GetTopicByIdQueryHandler CreateGetTopicByIdHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out Domain.Entities.ConnectionTopic topic)
        {
            topic = new Domain.Entities.ConnectionTopic
            {
                Id = 1,
                Topic = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var myProfile = new ConnectionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetTopicByIdQueryHandler>>();

            return new GetTopicByIdQueryHandler(connectionRepositoryMock.Object, mapper, loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetTopicById_ShouldReturnTopic_WhenTopicExists()
        {
            // arrange
            var handler = CreateGetTopicByIdHandler(out var connectionRepositoryMock, out var topic);
            var query = new GetTopicByIdQuery(topic.Id);

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id)).ReturnsAsync(topic);

            // act
            var result = await handler.Handle(query, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetTopicById(topic.Id), Times.Once());
            result.Should().NotBeNull();
            result.Id.Should().Be(topic.Id);
            result.Topic.Should().Be(topic.Topic);
        }

        [Fact]
        public async Task Handle_GetTopicById_ShouldThrowNotFoundException_WhenTopicDoesNotExist()
        {
            // arrange
            var handler = CreateGetTopicByIdHandler(out var connectionRepositoryMock, out var topic);
            var query = new GetTopicByIdQuery(topic.Id);

            connectionRepositoryMock.Setup(c => c.GetTopicById(topic.Id)).ReturnsAsync((Domain.Entities.ConnectionTopic)null);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}