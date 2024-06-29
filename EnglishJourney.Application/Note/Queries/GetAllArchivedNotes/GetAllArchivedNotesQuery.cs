using MediatR;

namespace EnglishJourney.Application.Note.Queries.GetAllArchivedNotes
{
    public class GetAllArchivedNotesQuery : IRequest<IEnumerable<NoteDto>>
    {
    }
}