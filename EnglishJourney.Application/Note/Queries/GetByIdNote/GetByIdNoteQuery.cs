using MediatR;

namespace EnglishJourney.Application.Note.Queries.GetByIdNote
{
    public class GetByIdNoteQuery(int id) : IRequest<NoteDto>
    {
        public int Id { get; set; } = id;
    }
}