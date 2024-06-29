using EnglishJourney.Application.Connection;
using EnglishJourney.Application.Connection.Commands.CreateConnectionAttribute;
using EnglishJourney.Application.Connection.Commands.CreateConnectionTopic;
using EnglishJourney.Application.Connection.Commands.DeleteAttribute;
using EnglishJourney.Application.Connection.Commands.DeleteTopic;
using EnglishJourney.Application.Connection.Commands.EditConnectionTopic;
using EnglishJourney.Application.Connection.Queries.GetAllConnectionTopics;
using EnglishJourney.Application.Connection.Queries.GetByTopicId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishJourney.API.Controllers
{
    [ApiController]
    [Route("api/connections")]
    [Authorize]
    public class ConnectionController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectionTopicDto>>> GetAll([FromQuery] GetAllTopicsQuery query)
        {
            var topics = await mediator.Send(query);

            return Ok(topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConnectionTopicDto>> GetById(int id)
        {
            var topic = await mediator.Send(new GetTopicByIdQuery(id));

            return Ok(topic);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTopic(int id, EditTopicCommand command)
        {
            command.Id = id;
            await mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTopic(int id)
        {
            await mediator.Send(new DeleteTopicCommand { Id = id });

            return NoContent();
        }

        [HttpDelete("attributes/{attributeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAttribute(int attributeId)
        {
            await mediator.Send(new DeleteAttributeCommand { Id = attributeId });

            return NoContent();
        }

        [HttpPost()]
        public async Task<ActionResult<int>> CreateTopic(CreateTopicCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPost("{topicId}/attributes")]
        public async Task<ActionResult<int>> CreateAttribute([FromRoute] int topicId, CreateAttributeCommand command)
        {
            command.TopicId = topicId;
            var attributeId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = topicId }, null);
        }
    }
}