using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler(INoteRepository repository, ILogger<DeleteNoteCommandHandler> logger)
        : IRequestHandler<DeleteNoteCommand>
    {
        public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting note with id: {NoteId}", request.Id);

            var note = await repository.GetById(request.Id);
            if (note == null) throw new NotFoundException(nameof(Domain.Entities.Note), request.Id.ToString());

            await repository.Delete(note);
        }
    }
}