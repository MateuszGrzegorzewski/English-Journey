using MediatR;

namespace EnglishJourney.Application.Note.Commands.CreateNote
{
    public class CreateNoteCommand : NoteDto, IRequest<int>
    {
    }
}