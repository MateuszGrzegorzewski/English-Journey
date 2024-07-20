using MediatR;

namespace EnglishJourney.Application.Statistic.Queries.GetUserStatistics
{
    public class GetUserStatisticsQuery : IRequest<IEnumerable<UserStatisticDto>>
    {
    }
}