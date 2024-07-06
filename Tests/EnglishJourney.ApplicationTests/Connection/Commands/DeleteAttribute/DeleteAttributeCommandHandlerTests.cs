using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EnglishJourney.Application.Connection.Commands.DeleteAttribute.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeleteAttributeCommandHandlerTests
    {
        private DeleteAttributeCommandHandler CreateDeleteAttributeHandler(out Mock<IConnectionRepository> connectionRepositoryMock, out ConnectionAttribute attribute, bool service = true)
        {
            attribute = new ConnectionAttribute
            {
                Id = 1,
                Word = "Test"
            };

            connectionRepositoryMock = new Mock<IConnectionRepository>();

            var loggerMock = new Mock<ILogger<DeleteAttributeCommandHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeConnection(It.IsAny<ConnectionTopic>(), It.IsAny<ResourceOperation>())).Returns(service);

            return new DeleteAttributeCommandHandler(connectionRepositoryMock.Object, loggerMock.Object, englishJourneyAuthorizationServiceMock.Object);
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

        [Fact]
        public async Task Handle_DeleteAttribute_ShouldThrowForbidException_WhenNoAuthorized()
        {
            // arrange
            var handler = CreateDeleteAttributeHandler(out var connectionRepositoryMock, out var attribute, false);
            var command = new DeleteAttributeCommand
            {
                Id = attribute.Id
            };

            connectionRepositoryMock.Setup(c => c.GetAttributeById(attribute.Id)).ReturnsAsync(attribute);

            // act & assert
            await Assert.ThrowsAsync<ForbidException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}