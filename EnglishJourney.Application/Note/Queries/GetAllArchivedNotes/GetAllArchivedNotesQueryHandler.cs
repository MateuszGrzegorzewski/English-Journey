using AutoMapper;
using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Queries.GetAllArchivedNotes
{
    public class GetAllArchivedNotesQueryHandler(INoteRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService, IUserContext userContext,
        ILogger<GetAllArchivedNotesQueryHandler> logger)
        : IRequestHandler<GetAllArchivedNotesQuery, IEnumerable<NoteDto>>
    {
        public async Task<IEnumerable<NoteDto>> Handle(GetAllArchivedNotesQuery request, CancellationToken cancellation)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException();

            logger.LogInformation("Getting all archived notes");

            var notes = await repository.GetAllArchived(currentUser.Id);
            var dtos = mapper.Map<IEnumerable<NoteDto>>(notes);

            if (notes != null && notes.ToList().Count > 0 && !authorizationService.AuthorizeNotes(notes.ToList()[0], ResourceOperation.ReadAll))
                throw new ForbidException();

            return dtos;
        }
    }
}