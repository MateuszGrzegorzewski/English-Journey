using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Interfaces;
using FluentValidation;

namespace EnglishJourney.Application.Flashcard.Commands.EditCategory
{
    public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
    {
        public EditCategoryCommandValidator(IFlashcardRepository repository, IUserContext userContext)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User is not authenticated");

            RuleFor(f => f.Name)
                .NotEmpty()
                .MaximumLength(20).WithMessage("Too much characters")
                .Custom((name, context) =>
                {
                    var existingName = repository.GetFlashcardCategoryByName(name, currentUser.Id).Result;
                    if (existingName != null)
                    {
                        context.AddFailure($"{name} is not unique name for category");
                    }
                });
        }
    }
}