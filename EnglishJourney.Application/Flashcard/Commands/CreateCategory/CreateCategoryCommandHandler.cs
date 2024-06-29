using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Flashcard.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(IFlashcardRepository repository, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger)
        : IRequestHandler<CreateCategoryCommand, int>
    {
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating flashcard category {@FlashcardCategory}", request);

            var category = mapper.Map<Domain.Entities.FlashcardCategory>(request);

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