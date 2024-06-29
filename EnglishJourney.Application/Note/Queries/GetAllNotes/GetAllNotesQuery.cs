using MediatR;

namespace EnglishJourney.Application.Note.Query.GetAllNotes
{
    public class GetAllNotesQuery : IRequest<IEnumerable<NoteDto>>
    {
    }
}