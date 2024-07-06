using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Queries.GetCategoryById.Tests
{
    public class GetCategoryByIdQueryHandlerTests
    {
        private GetCategoryByIdQueryHandler CreateGetCategoryByIdHandler(out Mock<IFlashcardRepository> flashcardRepositoryMock, out Domain.Entities.FlashcardCategory category)
        {
            category = new Domain.Entities.FlashcardCategory
            {
                Id = 1,
                Name = "Test"
            };

            flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetCategoryByIdQueryHandler>>();

            var englishJourneyAuthorizationServiceMock = new Mock<IEnglishJourneyAuthorizationService>();
            englishJourneyAuthorizationServiceMock.Setup(e => e.AuthorizeFlashcard(It.IsAny<FlashcardCategory>(), It.IsAny<ResourceOperation>())).Returns(true);

            return new GetCategoryByIdQueryHandler(flashcardRepositoryMock.Object, mapper, englishJourneyAuthorizationServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task HandleGetCategoryById_ShouldReturnCategory_WhenCategoryExists()
        {
            // arrange
            var handler = CreateGetCategoryByIdHandler(out var flashcardRepositoryMock, out var category);
            var query = new GetCategoryByIdQuery(category.Id);

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id)).ReturnsAsync(category);

            // act
            var result = await handler.Handle(query, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetFlashardCategoryById(category.Id), Times.Once());
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
            result.Name.Should().Be(category.Name);
        }

        [Fact]
        public async Task HandleGetCategoryById_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            // arrange
            var handler = CreateGetCategoryByIdHandler(out var flashcardRepositoryMock, out var category);
            var query = new GetCategoryByIdQuery(category.Id);

            flashcardRepositoryMock.Setup(f => f.GetFlashardCategoryById(category.Id));

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}