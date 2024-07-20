using EnglishJourney.Domain.Statistic;
using MediatR;

namespace EnglishJourney.Application.Statistic.Queries.GetDemography
{
    public class GetDemographyQuery : IRequest<List<DemographyResult>>
    {
    }
}