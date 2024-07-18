using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Domain.Interfaces
{
    public interface IUserStatisticRepository
    {
        Task<int> AdminRoles();

        Task Create(UserStatistic userStatistic);

        Task<int> NationalitiesSet();

        Task<int> RegisteredAccounts();
    }
}