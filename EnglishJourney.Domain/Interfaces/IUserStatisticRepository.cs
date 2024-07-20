using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Statistic;

namespace EnglishJourney.Domain.Interfaces
{
    public interface IUserStatisticRepository
    {
        Task<int> AdminRoles();

        Task Create(UserStatistic userStatistic);

        Task<int> NationalitiesSet();

        Task<int> RegisteredAccounts();

        Task<IEnumerable<UserStatistic>> GetAllUserStatitistics();

        Task<List<DemographyResult>> GetDemography();
    }
}