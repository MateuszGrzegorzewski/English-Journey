using EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute;
using FluentValidation;

namespace EnglishJourney.Application.Connection.Commands.CreateAttribute
{
    public class CreateAttributeCommandValidation : AbstractValidator<CreateAttributeCommand>
    {
        public CreateAttributeCommandValidation()
        {
            RuleFor(c => c.Word)
                .NotEmpty()
                .MaximumLength(32).WithMessage("To much characters");

            RuleFor(c => c.Definition)
                .MaximumLength(256).WithMessage("To much characters");
        }
    }
}