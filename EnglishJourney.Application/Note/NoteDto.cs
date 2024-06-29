namespace EnglishJourney.Application.Note
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }
}