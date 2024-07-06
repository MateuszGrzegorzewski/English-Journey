using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnglishJourney.Infrastructure.Repositories
{
    public class FlashcardRepository : IFlashcardRepository
    {
        private readonly EnglishJourneyDbContext dbContext;

        public FlashcardRepository(EnglishJourneyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Commit()
            => await dbContext.SaveChangesAsync();

        public async Task CreateFlashcard(Flashcard flashcard)
        {
            dbContext.Add(flashcard);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateFlashcardBox(FlashcardBox flashcardBox)
        {
            dbContext.Add(flashcardBox);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateFlashcardCategory(FlashcardCategory flashcardCategory)
        {
            dbContext.Add(flashcardCategory);
            await dbContext.SaveChangesAsync();

            return flashcardCategory.Id;
        }

        public async Task DeleteFlashcard(Flashcard flashcard)
        {
            dbContext.Remove(flashcard);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteFlashcardCategory(FlashcardCategory flashcardCategory)
        {
            dbContext.Remove(flashcardCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<FlashcardCategory>?> GetAllFlashcardCategories(string userId)
            => await dbContext.FlashcardsCategories.Where(fc => fc.UserId == userId).OrderByDescending(f => f.CreatedAt).ToListAsync();

        public async Task<FlashcardBox?> GetFlashardBoxById(int flashcardBoxId)
            => await dbContext.FlashcardsBoxes.Include(f => f.Flashcards).Include(f => f.FlashcardCategory).Where(f => f.Id == flashcardBoxId).FirstOrDefaultAsync();

        public async Task<FlashcardBox?> GetFlashcardBoxByCategoryIdAndBoxNumber(int categoryId, int boxNumber)
            => await dbContext.FlashcardsBoxes.FirstOrDefaultAsync(f => f.FlashcardCategoryId == categoryId && f.BoxNumber == boxNumber);

        public async Task<Flashcard?> GetFlashardById(int flashcardId)
            => await dbContext.Flashcards.Include(f => f.FlashcardBox).ThenInclude(fb => fb.FlashcardCategory).Where(f => f.Id == flashcardId).FirstOrDefaultAsync();

        public async Task<FlashcardCategory?> GetFlashardCategoryById(int flashcardCategoryId)
            => await dbContext.FlashcardsCategories.Include(f => f.FlashcardBoxes).ThenInclude(f => f.Flashcards)
                                                   .Where(f => f.Id == flashcardCategoryId).FirstOrDefaultAsync();

        public async Task<FlashcardCategory?> GetFlashcardCategoryByName(string flaschardCategoryName, string userId)
            => await dbContext.FlashcardsCategories.Where(f => f.Name == flaschardCategoryName).Where(f => f.UserId == userId).FirstOrDefaultAsync();
    }
}