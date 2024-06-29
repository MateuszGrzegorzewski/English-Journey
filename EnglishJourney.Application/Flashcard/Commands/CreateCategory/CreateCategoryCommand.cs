using MediatR;

namespace EnglishJourney.Application.Flashcard.Commands.CreateCategory
{
    public class CreateCategoryCommand : FlashcardCategoryDto, IRequest<int>
    {
    }
}