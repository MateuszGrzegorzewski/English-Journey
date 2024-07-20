using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Statistic.Queries.GetUserStatistics;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.ApplicationTests.Statistic.Queries.GetUserStatistics
{
    public class GetUserStatisticsHandlerTests
    {
        [Fact]
        public async void Handle_GetUserStatistics()
        {
            // arrange
            var query = new GetUserStatisticsQuery();

            var repositoryMock = new Mock<IUserStatisticRepository>();

            var myProfile = new StatisticMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetUserStatisticsQueryHandler>>();

            var handler = new GetUserStatisticsQueryHandler(repositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            repositoryMock.Verify(r => r.GetAllUserStatitistics(), Times.Once);
        }
    }
}