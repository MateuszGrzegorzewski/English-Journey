using EnglishJourney.Application.Statistic;
using EnglishJourney.Application.Statistic.Queries.GetDemography;
using EnglishJourney.Application.Statistic.Queries.GetUserStatistics;
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

        [HttpGet("demography")]
        public async Task<ActionResult<List<(string? Nationality, int Count)>>> GetDemography([FromQuery] GetDemographyQuery query)
        {
            var demography = await mediator.Send(query);

            return Ok(demography);
        }
    }
}