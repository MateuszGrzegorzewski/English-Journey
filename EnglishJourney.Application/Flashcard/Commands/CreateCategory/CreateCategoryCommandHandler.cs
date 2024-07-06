using AutoMapper;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(IFlashcardRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService, IUserContext userContext,
        ILogger<CreateCategoryCommandHandler> logger)
        : IRequestHandler<CreateCategoryCommand, int>
    {
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User is not authenticated");

            logger.LogInformation("Creating flashcard category {@FlashcardCategory}", request);

            var category = mapper.Map<Domain.Entities.FlashcardCategory>(request);
            category.UserId = currentUser.Id;

            if (!authorizationService.AuthorizeFlashcard(category, ResourceOperation.Create))
                throw new ForbidException();

            var categoryId = await repository.CreateFlashcardCategory(category);

            for (var boxNumber = 1; boxNumber <= 6; boxNumber++)
            {
                var box = new Domain.Entities.FlashcardBox();
                box.BoxNumber = boxNumber;
                box.FlashcardCategoryId = category.Id;

                await repository.CreateFlashcardBox(box);
            }

            return categoryId;
        }
    }
}