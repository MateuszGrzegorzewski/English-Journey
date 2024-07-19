using EnglishJourney.Application.Statistic;
using EnglishJourney.Application.Statistic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnglishJourney.API.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticController(IMediator mediator) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserStatisticDto>>> GetAllUserStatistics([FromQuery] GetUserStatisticsQuery query)
        {
            var userStatistics = await mediator.Send(query);

            return Ok(userStatistics);
        }
    }
}