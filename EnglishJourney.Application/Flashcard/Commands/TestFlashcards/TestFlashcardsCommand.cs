using MediatR;

namespace EnglishJourney.Application.Flashcard.Commands.TestFlashcards
{
    public class TestFlashcardsCommand : FlashcardDto, IRequest
    {
        public Dictionary<int, bool>? TestResults { get; set; }
    }
}