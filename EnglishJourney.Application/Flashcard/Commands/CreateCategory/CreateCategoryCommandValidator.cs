using EnglishJourney.Domain.Interfaces;
using FluentValidation;

namespace EnglishJourney.Application.Flashcard.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(IFlashcardRepository repository)
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