using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Queries.GetAllConnectionTopics.Tests
{
    public class GetAllTopicsQueryHandlerTests
    {
        [Fact()]
        public async void Handle_GetAllTopics()
        {
            // arrange
            var query = new GetAllTopicsQuery();

            var connectionRepositoryMock = new Mock<IConnectionRepository>();

            var myProfile = new ConnectionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllTopicsQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllTopicsQueryHandler(connectionRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object, userContextMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetAllTopics(currentUser.Id), Times.Once());
        }

        [Fact()]
        public async void Handle_GetAllTopics_ShouldThrowException_WhenUserIsNull()
        {
            // arrange
            var query = new GetAllTopicsQuery();

            var connectionRepositoryMock = new Mock<IConnectionRepository>();

            var myProfile = new ConnectionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllTopicsQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(true);

            var handler = new GetAllTopicsQueryHandler(connectionRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object, userContextMock.Object);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact()]
        public async void Handle_GetAllTopics_ShouldThrowForbidException_WhenNoAuthorized()
        {
            // arrange
            var query = new GetAllTopicsQuery();

            var connectionRepositoryMock = new Mock<IConnectionRepository>();
            connectionRepositoryMock.Setup(c => c.GetAllTopics(It.IsAny<string>()))
                .ReturnsAsync(new List<ConnectionTopic> { new ConnectionTopic { Topic = "Test topic" } });

            var myProfile = new ConnectionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllTopicsQueryHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("user-id", "test@test.com", [], null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(false);

            var handler = new GetAllTopicsQueryHandler(connectionRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object, userContextMock.Object);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}