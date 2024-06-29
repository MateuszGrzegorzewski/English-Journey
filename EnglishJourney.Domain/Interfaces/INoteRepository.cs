using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Domain.Interfaces
{
    public interface INoteRepository
    {
        Task Commit();

        Task<int> Create(Note note);

        Task Delete(Note note);

        Task<IEnumerable<Note>?> GetAll();

        Task<IEnumerable<Note>?> GetAllArchived();

        Task<Note?> GetById(int id);
    }
}