using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Domain.Statistic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Statistic.Queries.GetDemography
{
    public class GetDemographyQueryHandler(IUserStatisticRepository repository, ILogger<GetDemographyQueryHandler> logger)
        : IRequestHandler<GetDemographyQuery, List<DemographyResult>>
    {
        public async Task<List<DemographyResult>> Handle(GetDemographyQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get demography of users");
            return await repository.GetDemography();
        }
    }
}