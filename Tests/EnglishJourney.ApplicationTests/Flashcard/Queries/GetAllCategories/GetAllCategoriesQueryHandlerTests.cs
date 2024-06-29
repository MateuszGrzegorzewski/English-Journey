using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Flashcard.Queries.GetAllCategories.Tests
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact()]
        public async void Handle_GetAllCategories()
        {
            // arrange
            var query = new GetAllCategoriesQuery();

            var flashcardRepositoryMock = new Mock<IFlashcardRepository>();

            var myProfile = new FlashcardMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllCategoriesQueryHandler>>();

            var handler = new GetAllCategoriesQueryHandler(flashcardRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            flashcardRepositoryMock.Verify(f => f.GetAllFlashcardCategories(), Times.Once);
        }
    }
}