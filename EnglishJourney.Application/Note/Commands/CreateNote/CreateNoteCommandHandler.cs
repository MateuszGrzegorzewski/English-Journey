using AutoMapper;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Commands.CreateNote
{
    public class CreateNoteCommandHandler(INoteRepository repository, IMapper mapper,
        IUserContext userContext, IEnglishJourneyAuthorizationService authorizationService,
        ILogger<CreateNoteCommandHandler> logger)
        : IRequestHandler<CreateNoteCommand, int>
    {
        public async Task<int> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating note {@Note}", request);

            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException();

            var note = mapper.Map<Domain.Entities.Note>(request);
            note.UserId = currentUser.Id;

            if (!authorizationService.AuthorizeNotes(note, ResourceOperation.Create))
                throw new ForbidException();

            return await repository.Create(note);
        }
    }
}