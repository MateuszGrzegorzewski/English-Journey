using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Commands.EditNote
{
    public class EditNoteCommandHandler(INoteRepository repository, ILogger<EditNoteCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<EditNoteCommand>
    {
        public async Task Handle(EditNoteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Editing note {@Note} with id: {NoteId}", request, request.Id);

            var note = await repository.GetById(request.Id);
            if (note == null) throw new NotFoundException(nameof(Domain.Entities.Note), request.Id.ToString());

            note.Title = request.Title;
            note.Description = request.Description;
            note.LastModified = DateTime.UtcNow;

            if (!authorizationService.AuthorizeNotes(note, ResourceOperation.Update))
                throw new ForbidException();

            await repository.Commit();
        }
    }
}