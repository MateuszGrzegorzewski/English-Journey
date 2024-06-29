using EnglishJourney.Domain.Interfaces;
using FluentValidation;

namespace EnglishJourney.Application.Flashcard.Commands.EditCategory
{
    public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
    {
        public EditCategoryCommandValidator(IFlashcardRepository repository)
        {
            RuleFor(f => f.Name)
                .NotEmpty()
                .MaximumLength(20).WithMessage("Too much characters")
                .Custom((name, context) =>
                {
                    var existingName = repository.GetFlashcardCategoryByName(name).Result;
                    if (existingName != null)
                    {
                        context.AddFailure($"{name} is not unique name for category");
                    }
                });
        }
    }
}