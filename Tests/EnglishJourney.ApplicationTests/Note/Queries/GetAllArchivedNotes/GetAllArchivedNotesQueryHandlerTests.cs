using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Queries.GetAllArchivedNotes.Tests
{
    public class GetAllArchivedNotesQueryHandlerTests
    {
        [Fact()]
        public async void Handle_GetArchivedNotes()
        {
            // arrange
            var query = new GetAllArchivedNotesQuery();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllArchivedNotesQueryHandler>>();

            var handler = new GetAllArchivedNotesQueryHandler(noteRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetAllArchived(), Times.Once);
        }
    }
}