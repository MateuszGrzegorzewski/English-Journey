using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnglishJourney.Infrastructure.Repositories
{
    public class UserStatisticRepository(EnglishJourneyDbContext dbContext) : IUserStatisticRepository
    {
        private readonly EnglishJourneyDbContext dbContext = dbContext;

        public async Task Create(UserStatistic userStatistic)
        {
            dbContext.Add(userStatistic);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> RegisteredAccounts()
            => await dbContext.Users.CountAsync();

        public async Task<int> AdminRoles()
        {
            var adminRole = await dbContext.Roles.FirstOrDefaultAsync(u => u.NormalizedName == "ADMIN");
            return adminRole != null ? await dbContext.UserRoles.CountAsync(u => u.RoleId == adminRole.Id) : 0;
        }

        public async Task<int> NationalitiesSet()
            => await dbContext.Users.CountAsync(u => u.Nationality != null);
    }
}