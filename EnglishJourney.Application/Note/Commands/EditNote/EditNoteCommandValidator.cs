using FluentValidation;

namespace EnglishJourney.Application.Note.Commands.EditNote
{
    public class EditNoteCommandValidator : AbstractValidator<EditNoteCommand>
    {
        public EditNoteCommandValidator()
        {
            RuleFor(n => n.Title)
                .NotEmpty()
                .Length(2, 64).WithMessage("Inappropriate number of characters");

            RuleFor(n => n.Description)
                .MaximumLength(500).WithMessage("Too much characters");
        }
    }
}