using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnglishJourney.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly EnglishJourneyDbContext dbContext;

        public NoteRepository(EnglishJourneyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Commit()
            => await dbContext.SaveChangesAsync();

        public async Task<int> Create(Note note)
        {
            dbContext.Add(note);
            await dbContext.SaveChangesAsync();

            return note.Id;
        }

        public async Task Delete(Note note)
        {
            dbContext.Remove(note);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Note>?> GetAll()
            => await dbContext.Notes.Where(n => n.IsArchivized == false).OrderByDescending(n => n.LastModified).ToListAsync();

        public async Task<IEnumerable<Note>?> GetAllArchived()
            => await dbContext.Notes.Where(n => n.IsArchivized == true).OrderByDescending(n => n.LastModified).ToListAsync();

        public async Task<Note?> GetById(int id)
            => await dbContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
    }
}