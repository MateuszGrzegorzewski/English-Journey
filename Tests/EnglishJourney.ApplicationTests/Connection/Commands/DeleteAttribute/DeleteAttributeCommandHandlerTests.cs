﻿using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.DeleteAttribute.Tests
{
    public class DeleteAttributeCommandHandlerTests
    {
        private DeleteAttributeCommandHandler CreateDeleteAttributeHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out Domain.Entities.ConnectionAttribute attribute)
        {
            attribute = new Domain.Entities.ConnectionAttribute
            {
                Id = 1,
                Word = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var loggerMock = new Mock<ILogger<DeleteAttributeCommandHandler>>();

            return new DeleteAttributeCommandHandler(connectionRepositoryMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteAttribute_ShouldDeleteAttribute_WhenAttributeExists()
        {
            // arrange
            var handler = CreateDeleteAttributeHandler(out var connectionRepositoryMock, out var attribute);
            var command = new DeleteAttributeCommand
            {
                Id = attribute.Id
            };

            connectionRepositoryMock.Setup(c => c.GetAttributeById(attribute.Id)).ReturnsAsync(attribute);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            connectionRepositoryMock.Verify(c => c.GetAttributeById(attribute.Id), Times.Once);
            connectionRepositoryMock.Verify(c => c.DeleteAttribute(It.Is<Domain.Entities.ConnectionAttribute>(a => a.Id == attribute.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteAttribute_ShouldThrowNotFoundException_WhenAttributeDoesNotExist()
        {
            // arrange
            var handler = CreateDeleteAttributeHandler(out var connectionRepositoryMock, out var attribute);
            var command = new DeleteAttributeCommand
            {
                Id = attribute.Id
            };

            connectionRepositoryMock.Setup(c => c.GetAttributeById(attribute.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}