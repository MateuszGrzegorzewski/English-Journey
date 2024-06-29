using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Queries.GetAllArchivedNotes
{
    public class GetAllArchivedNotesQueryHandler(INoteRepository repository, IMapper mapper, ILogger<GetAllArchivedNotesQueryHandler> logger)
        : IRequestHandler<GetAllArchivedNotesQuery, IEnumerable<NoteDto>>
    {
        public async Task<IEnumerable<NoteDto>> Handle(GetAllArchivedNotesQuery request, CancellationToken cancellation)
        {
            logger.LogInformation("Getting all archived notes");

            var notes = await repository.GetAllArchived();
            var dtos = mapper.Map<IEnumerable<NoteDto>>(notes);

            return dtos;
        }
    }
}