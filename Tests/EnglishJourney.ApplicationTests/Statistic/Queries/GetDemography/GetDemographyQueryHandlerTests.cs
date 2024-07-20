using EnglishJourney.Application.Statistic.Queries.GetDemography;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.ApplicationTests.Statistic.Queries.GetDemography
{
    public class GetDemographyQueryHandlerTests
    {
        [Fact]
        public async void Handle_GetDemography()
        {
            // arrange
            var query = new GetDemographyQuery();

            var repositoryMock = new Mock<IUserStatisticRepository>();

            var loggerMock = new Mock<ILogger<GetDemographyQueryHandler>>();

            var handler = new GetDemographyQueryHandler(repositoryMock.Object, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            repositoryMock.Verify(r => r.GetDemography(), Times.Once);
        }
    }
}