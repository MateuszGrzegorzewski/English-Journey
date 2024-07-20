using AutoMapper;
using EnglishJourney.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Statistic.Queries.GetUserStatistics
{
    internal class GetUserStatisticsQueryHandler(IUserStatisticRepository repository, IMapper mapper,
        ILogger<GetUserStatisticsQueryHandler> logger)
        : IRequestHandler<GetUserStatisticsQuery, IEnumerable<UserStatisticDto>>
    {
        public async Task<IEnumerable<UserStatisticDto>> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all user statistics");

            var statistics = await repository.GetAllUserStatitistics();
            var dtos = mapper.Map<IEnumerable<UserStatisticDto>>(statistics);

            return dtos;
        }
    }
}