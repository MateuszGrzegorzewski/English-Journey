using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionTopic.Tests
{
    public class CreateTopicCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateTopic()
        {
            // arrange
            var command = new CreateTopicCommand();

            var connectionRepositoryMock = new Mock<IConnectionRepository>();

            var myProfile = new ConnectionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateTopicCommandHandler>>();

            var handler = new CreateTopicCommandHandler(connectionRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.CreateTopic(It.IsAny<Domain.Entities.ConnectionTopic>()), Times.Once);
        }
    }
}