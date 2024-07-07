using EnglishJourney.Application.Flashcard;
using EnglishJourney.Application.Flashcard.Commands.CreateCategory;
using EnglishJourney.Application.Flashcard.Commands.CreateFlashcard;
using EnglishJourney.Application.Flashcard.Commands.DeleteCategory;
using EnglishJourney.Application.Flashcard.Commands.DeleteFlashcard;
using EnglishJourney.Application.Flashcard.Commands.EditCategory;
using EnglishJourney.Application.Flashcard.Commands.TestFlashcards;
using EnglishJourney.Application.Flashcard.Queries.GetAllCategories;
using EnglishJourney.Application.Flashcard.Queries.GetBoxById;
using EnglishJourney.Application.Flashcard.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishJourney.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/flashcards")]
    public class FlashcardController(IMediator mediator) : ControllerBase
    {
        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<FlashcardCategoryDto>>> GetAllCategories([FromQuery] GetAllCategoriesQuery query)
        {
            var categories = await mediator.Send(query);

            return Ok(categories);
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<FlashcardCategoryDto>> GetCategoryById(int id)
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(id));

            return Ok(category);
        }

        [HttpPatch("category/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCategory(int id, EditCategoryCommand command)
        {
            command.Id = id;
            await mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("category/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await mediator.Send(new DeleteCategoryCommand { Id = id });

            return NoContent();
        }

        [HttpPost("category")]
        public async Task<ActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetCategoryById), new { id }, null);
        }

        [HttpGet("box/{id}")]
        public async Task<ActionResult<FlashcardBoxDto>> GetBoxById(int id)
        {
            var box = await mediator.Send(new GetBoxByIdQuery(id));

            return Ok(box);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteFlashcard(int id)
        {
            await mediator.Send(new DeleteFlashcardCommand { Id = id });

            return NoContent();
        }

        [HttpPost("box/{id}")]
        public async Task<ActionResult> CreateFlashcard(int id, CreateFlashcardCommand command)
        {
            command.FlashcardBoxId = id;
            await mediator.Send(command);

            return CreatedAtAction(nameof(GetBoxById), new { id }, null);
        }

        [HttpPatch("box/{boxId}/test")]
        public async Task<ActionResult> TestFlashcards([FromBody] Dictionary<int, bool> testResults, [FromQuery] int boxId)
        {
            var command = new TestFlashcardsCommand
            {
                TestResults = testResults
            };

            await mediator.Send(command);

            return CreatedAtAction(nameof(GetBoxById), new { id = boxId }, null);
        }
    }
}