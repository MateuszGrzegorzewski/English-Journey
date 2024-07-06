using EnglishJourney.Application.Note;
using EnglishJourney.Application.Note.Commands.ArchiveNote;
using EnglishJourney.Application.Note.Commands.CreateNote;
using EnglishJourney.Application.Note.Commands.DeArchiveNote;
using EnglishJourney.Application.Note.Commands.DeleteNote;
using EnglishJourney.Application.Note.Commands.EditNote;
using EnglishJourney.Application.Note.Queries.GetAllArchivedNotes;
using EnglishJourney.Application.Note.Query.GetAllNotes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishJourney.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notes")]
    public class NoteController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll([FromQuery] GetAllNotesQuery query)
        {
            var notes = await mediator.Send(query);

            return Ok(notes);
        }

        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllArchived([FromQuery] GetAllArchivedNotesQuery query)
        {
            var notes = await mediator.Send(query);

            return Ok(notes);
        }

        [HttpPatch("{id}/archive")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Archive(int id)
        {
            await mediator.Send(new ArchiveNoteCommand { Id = id });

            return NoContent();
        }

        [HttpPatch("{id}/dearchive")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeArchive(int id)
        {
            await mediator.Send(new DeArchiveNoteCommand { Id = id });

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, EditNoteCommand command)
        {
            command.Id = id;
            await mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteNoteCommand { Id = id });

            return NoContent();
        }

        [HttpPost()]
        public async Task<ActionResult<int>> Create(CreateNoteCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), null);
        }
    }
}