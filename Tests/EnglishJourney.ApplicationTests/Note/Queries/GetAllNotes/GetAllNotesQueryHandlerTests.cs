using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Query.GetAllNotes.Tests
{
    public class GetAllNotesQueryHandlerTests
    {
        [Fact()]
        public async void Handle_GetAllNotes()
        {
            // arrange
            var query = new GetAllNotesQuery();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<GetAllNotesQueryHandler>>();

            var handler = new GetAllNotesQueryHandler(noteRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(query, CancellationToken.None);

            // assert
            noteRepositoryMock.Verify(n => n.GetAll(), Times.Once);
        }
    }
}