﻿using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute.Tests
{
    public class CreateAttributeCommandHandlerTests
    {
        private CreateAttributeCommandHandler CreateCreateAttributeHandler(out Mock<IConnectionRepository> connectionRepositoryMock)
        {
            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var myProfile = new ConnectionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateAttributeCommandHandler>>();

            return new CreateAttributeCommandHandler(connectionRepositoryMock.Object, mapper, loggerMock.Object);
        }

        [Fact]
        public async Task Handle_CreateAttribute_ShouldCreateAttribute()
        {
            // arrange
            var handler = CreateCreateAttributeHandler(out var connectionRepositoryMock);
            var command = new CreateAttributeCommand();

            connectionRepositoryMock.Setup(c => c.GetTopicById(It.IsAny<int>())).ReturnsAsync(new ConnectionTopic());

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetTopicById(It.IsAny<int>()), Times.Once);
            connectionRepositoryMock.Verify(c => c.CreateAttribute(It.IsAny<ConnectionAttribute>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateAttribute_ShouldThrowException_WhenTopicNotFound()
        {
            // arrange
            var handler = CreateCreateAttributeHandler(out var connectionRepositoryMock);
            var command = new CreateAttributeCommand();

            connectionRepositoryMock.Setup(c => c.GetTopicById(It.IsAny<int>()));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}