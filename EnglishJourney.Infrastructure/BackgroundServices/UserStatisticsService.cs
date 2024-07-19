using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Infrastructure.BackgroundServices
{
    public class UserStatisticsService(IUserStatisticRepository repository, ILogger<UserStatisticsService> logger)
        : IUserStatisticsService
    {
        public async Task GetUserStatisticAsync()
        {
            logger.LogInformation($"Running a background service to collect user statistics - {DateTime.Now.ToString("dd MMMM yyyy")}");

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
            logger.LogInformation($"End background service to collect user statistics - {DateTime.Now.ToString("dd MMMM yyyy")}");
        }
    }
}