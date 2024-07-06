using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Domain.Interfaces
{
    public interface INoteRepository
    {
        Task Commit();

        Task<int> Create(Note note);

        Task Delete(Note note);

        Task<IEnumerable<Note>?> GetAll(string userId);

        Task<IEnumerable<Note>?> GetAllArchived(string userId);

        Task<Note?> GetById(int id);
    }
}