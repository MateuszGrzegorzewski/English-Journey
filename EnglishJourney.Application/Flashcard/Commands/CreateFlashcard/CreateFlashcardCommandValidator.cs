using EnglishJourney.Domain.Interfaces;
using FluentValidation;

namespace EnglishJourney.Application.Flashcard.Commands.CreateFlashcard
{
    public class CreateFlashcardCommandValidator : AbstractValidator<CreateFlashcardCommand>
    {
        public CreateFlashcardCommandValidator(IFlashcardRepository repository)
        {
            RuleFor(f => f.Phrase)
                .NotEmpty()
                .Length(2, 128).WithMessage("Wrong number of characters");

            RuleFor(f => f.Definition)
                .NotEmpty()
                .Length(2, 256).WithMessage("Wrong number of characters");
        }
    }
}