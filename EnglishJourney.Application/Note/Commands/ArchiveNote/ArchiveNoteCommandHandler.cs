using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Commands.ArchiveNote
{
    public class ArchiveNoteCommandHandler(INoteRepository repository, ILogger<ArchiveNoteCommandHandler> logger,
        IEnglishJourneyAuthorizationService authorizationService)
        : IRequestHandler<ArchiveNoteCommand>
    {
        public async Task Handle(ArchiveNoteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Archiving note {@Note} with id: {NoteId}", request, request.Id);

            var note = await repository.GetById(request.Id);
            if (note == null) throw new NotFoundException(nameof(Domain.Entities.Note), request.Id.ToString());
            note.IsArchivized = true;
            note.LastModified = DateTime.UtcNow;

            if (!authorizationService.AuthorizeNotes(note, ResourceOperation.Update))
                throw new ForbidException();

            await repository.Commit();
        }
    }
}