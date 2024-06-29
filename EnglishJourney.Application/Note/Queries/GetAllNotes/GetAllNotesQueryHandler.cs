using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Query.GetAllNotes
{
    public class GetAllNotesQueryHandler(INoteRepository repository, IMapper mapper, ILogger<GetAllNotesQueryHandler> logger)
        : IRequestHandler<GetAllNotesQuery, IEnumerable<NoteDto>>
    {
        public async Task<IEnumerable<NoteDto>> Handle(GetAllNotesQuery request, CancellationToken cancellation)
        {
            logger.LogInformation("Getting all notes");

            var notes = await repository.GetAll();
            var dtos = mapper.Map<IEnumerable<NoteDto>>(notes);

            return dtos;
        }
    }
}