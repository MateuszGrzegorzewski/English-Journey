using AutoMapper;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Exceptions;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Queries.GetByIdNote
{
    public class GetByIdNoteQueryHandler(INoteRepository repository, IMapper mapper,
        IEnglishJourneyAuthorizationService authorizationService,
        ILogger<GetByIdNoteQueryHandler> logger)
        : IRequestHandler<GetByIdNoteQuery, NoteDto>
    {
        public async Task<NoteDto> Handle(GetByIdNoteQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting note by id {@Note}", request);

            var note = await repository.GetById(request.Id);
            if (note == null) throw new NotFoundException(nameof(Domain.Entities.Note), request.Id.ToString());
            var dto = mapper.Map<NoteDto>(note);

            if (!authorizationService.AuthorizeNotes(note, ResourceOperation.Read))
                throw new ForbidException();

            return dto;
        }
    }
}