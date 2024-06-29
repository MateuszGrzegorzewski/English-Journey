using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Domain.Interfaces
{
    public interface IFlashcardRepository
    {
        Task Commit();

        Task CreateFlashcard(Flashcard flashcard);

        Task CreateFlashcardBox(FlashcardBox flashcardBox);

        Task<int> CreateFlashcardCategory(FlashcardCategory flashcardCategory);

        Task DeleteFlashcard(Flashcard flashcard);

        Task DeleteFlashcardCategory(FlashcardCategory flashcardCategory);

        Task<IEnumerable<FlashcardCategory>?> GetAllFlashcardCategories();

        Task<Flashcard?> GetFlashardById(int flashcardId);

        Task<FlashcardBox?> GetFlashardBoxById(int flashcardBoxId);

        Task<FlashcardBox?> GetFlashcardBoxByCategoryIdAndBoxNumber(int categoryId, int boxNumber);

        Task<FlashcardCategory?> GetFlashardCategoryById(int flashcardCategoryId);

        Task<FlashcardCategory?> GetFlashcardCategoryByName(string flashcardCategoryName);
    }
}