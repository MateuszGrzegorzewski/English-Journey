using AutoMapper;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler(IFlashcardRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService, IUserContext userContext,
        ILogger<GetAllCategoriesQueryHandler> logger)
        : IRequestHandler<GetAllCategoriesQuery, IEnumerable<FlashcardCategoryDto>>
    {
        public async Task<IEnumerable<FlashcardCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User is not authenticated");

            logger.LogInformation("Getting all flashcard categories");

            var categories = await repository.GetAllFlashcardCategories(currentUser.Id);
            var dtos = mapper.Map<IEnumerable<FlashcardCategoryDto>>(categories);

            if (categories != null && categories.ToList().Count > 0 && !authorizationService.AuthorizeFlashcard(categories.ToList()[0], ResourceOperation.ReadAll))
                throw new ForbidException();

            return dtos;
        }
    }
}