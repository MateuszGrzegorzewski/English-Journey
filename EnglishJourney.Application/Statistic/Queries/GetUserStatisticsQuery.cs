using MediatR;

namespace EnglishJourney.Application.Statistic.Queries
{
    public class GetUserStatisticsQuery : IRequest<IEnumerable<UserStatisticDto>>
    {
    }
}