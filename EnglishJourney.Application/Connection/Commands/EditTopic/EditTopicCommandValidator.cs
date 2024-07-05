using EnglishJourney.Application.Connection.Commands.EditConnectionTopic;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Interfaces;
using FluentValidation;

namespace EnglishJourney.Application.Connection.Commands.EditTopic
{
    public class EditTopicCommandValidator : AbstractValidator<EditTopicCommand>
    {
        public EditTopicCommandValidator(IConnectionRepository repository, IUserContext userContext)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            RuleFor(c => c.Topic)
            .NotEmpty()
            .MaximumLength(20).WithMessage("Too much characters")
            .Custom((topic, context) =>
            {
                var existingTopics = repository.GetTopicsByName(topic, currentUser.Id).Result;
                if (existingTopics != null)
                {
                    context.AddFailure($"{topic} is not unique name for topic");
                }
            });
        }
    }
}