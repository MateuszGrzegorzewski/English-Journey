using AutoMapper;
using EnglishJourney.Application.Mappings;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnglishJourney.Application.Note.Commands.CreateNote.Tests
{
    public class CreateNoteCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateNote()
        {
            // arrange
            var command = new CreateNoteCommand();

            var noteRepositoryMock = new Mock<INoteRepository>();

            var myProfile = new NoteMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<CreateNoteCommandHandler>>();

            var handler = new CreateNoteCommandHandler(noteRepositoryMock.Object, mapper, loggerMock.Object);

            // act
            await handler.Handle(command, CancellationToken.None);

            //assert
            noteRepositoryMock.Verify(n => n.Create(It.IsAny<Domain.Entities.Note>()), Times.Once);
        }
    }
}