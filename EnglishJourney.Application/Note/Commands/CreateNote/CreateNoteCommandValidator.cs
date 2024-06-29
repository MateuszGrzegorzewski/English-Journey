using FluentValidation;

namespace EnglishJourney.Application.Note.Commands.CreateNote
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(n => n.Title)
                .NotEmpty()
                .Length(2, 64).WithMessage("Inappropriate number of characters");

            RuleFor(n => n.Description)
                .MaximumLength(500).WithMessage("Too much characters");
        }
    }
}