﻿using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
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

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", []);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new CreateTopicCommandHandler(connectionRepositoryMock.Object, mapper, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object, userContextMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.CreateTopic(It.IsAny<ConnectionTopic>()), Times.Once);
        }
    }
}