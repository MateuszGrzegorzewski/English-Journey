using MediatR;

namespace EnglishJourney.Application.Flashcard.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : FlashcardCategoryDto, IRequest
    {
    }
}