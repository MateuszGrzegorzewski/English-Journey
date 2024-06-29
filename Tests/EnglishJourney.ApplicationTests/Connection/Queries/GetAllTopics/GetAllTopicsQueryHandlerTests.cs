using Xunit;
using EnglishJourney.Application.Connection.Queries.GetAllConnectionTopics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Moq;
using Microsoft.Extensions.Logging;

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

            var handler = new GetAllTopicsQueryHandler(connectionRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetAllTopics(), Times.Once());
        }
    }
}