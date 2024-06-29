using EnglishJourney.Application.Connection.Commands.EditConnectionTopic;
using EnglishJourney.Domain.Interfaces;
using FluentValidation;

namespace EnglishJourney.Application.Connection.Commands.EditTopic
{
    public class EditTopicCommandValidator : AbstractValidator<EditTopicCommand>
    {
        public EditTopicCommandValidator(IConnectionRepository repository)
        {
            RuleFor(c => c.Topic)
            .NotEmpty()
            .MaximumLength(20).WithMessage("Too much characters")
            .Custom((topic, context) =>
            {
                var existingTopics = repository.GetTopicsByName(topic).Result;
                if (existingTopics != null)
                {
                    context.AddFailure($"{topic} is not unique name for topic");
                }
            });
        }
    }
}