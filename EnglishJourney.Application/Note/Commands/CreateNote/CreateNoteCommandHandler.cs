using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Note.Commands.CreateNote
{
    public class CreateNoteCommandHandler(INoteRepository repository, IMapper mapper, ILogger<CreateNoteCommandHandler> logger)
        : IRequestHandler<CreateNoteCommand, int>
    {
        public async Task<int> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating note {@Note}", request);

            var note = mapper.Map<Domain.Entities.Note>(request);

            return await repository.Create(note);
        }
    }
}