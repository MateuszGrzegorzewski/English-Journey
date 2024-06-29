using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Persistence;
using EnglishJourney.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishJourney.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EnglishJourneyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EnglishJourney"))
                .EnableSensitiveDataLogging());

            services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<EnglishJourneyDbContext>();

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IConnectionRepository, ConnectionRepository>();
            services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        }
    }
}