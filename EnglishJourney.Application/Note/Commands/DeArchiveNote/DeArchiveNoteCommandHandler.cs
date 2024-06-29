using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Commands.DeArchiveNote
{
    public class DeArchiveNoteCommandHandler(INoteRepository repository, ILogger<DeArchiveNoteCommandHandler> logger)
        : IRequestHandler<DeArchiveNoteCommand>
    {
        public async Task Handle(DeArchiveNoteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("De-archiving note {@Note} with id: {NoteId}", request, request.Id);

            var note = await repository.GetById(request.Id);
            if (note == null) throw new NotFoundException(nameof(Domain.Entities.Note), request.Id.ToString());

            note.IsArchivized = false;
            note.LastModified = DateTime.UtcNow;

            await repository.Commit();
        }
    }
}