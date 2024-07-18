using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Interfaces;

namespace EnglishJourney.Infrastructure.BackgroundServices
{
    public class UserStatisticsService(IUserStatisticRepository repository) : IUserStatisticsService
    {
        public async Task GetDUserStatisticAsync()
        {
            var registeredAccounts = await repository.RegisteredAccounts();
            var admins = await repository.AdminRoles();
            var nationalitiesSet = await repository.NationalitiesSet();

            var userStatistic = new UserStatistic
            {
                Date = DateTime.Now,
                RegisteredAccounts = registeredAccounts,
                Admins = admins,
                NationalitiesSet = nationalitiesSet
            };

            await repository.Create(userStatistic);
        }
    }
}