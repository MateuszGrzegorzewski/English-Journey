using MediatR;

namespace EnglishJourney.Application.Note.Commands.DeleteNote
{
    public class DeleteNoteCommand : NoteDto, IRequest
    {
    }
}