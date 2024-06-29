using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Queries.GetBoxById.Tests
{
    public class GetBoxByIdQueryHandlerTests
    {
        private GetBoxByIdQueryHandler CreateGetBoxByIdHandler(out Mock<IFlashcardRepository> flashcardRepositoryMock, out Domain.Entities.FlashcardBox box)
        {
            box = new Domain.Entities.FlashcardBox
            {
                Id = 1
            };

            flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetBoxByIdQueryHandler>>();

            return new GetBoxByIdQueryHandler(flashcardRepositoryMock.Object, mapper, loggerMock.Object);
        }

        [Fact]
        public async Task HandleGetBoxById_ShouldReturnBox_WhenBoxExists()
        {
            // arrange
            var handler = CreateGetBoxByIdHandler(out var flashcardRepositoryMock, out var box);
            var query = new GetBoxByIdQuery(box.Id);

            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(box.Id)).ReturnsAsync(box);

            // act
            var result = await handler.Handle(query, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardBoxById(box.Id), Times.Once());
            result.Should().NotBeNull();
            result.Id.Should().Be(box.Id);
        }

        [Fact]
        public async Task HandleGetBoxById_ShouldThrowNotFoundException_WhenBoxDoesNotExist()
        {
            // arrange
            var handler = CreateGetBoxByIdHandler(out var flashcardRepositoryMock, out var box);
            var query = new GetBoxByIdQuery(box.Id);

            flashcardRepositoryMock.Setup(f => f.GetFlashardBoxById(box.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}