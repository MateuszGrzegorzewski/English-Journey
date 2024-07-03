using EnglishJourney.Application.Infrastructure.Services;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Authorization;
using EnglishJourney.Infrastructure.Persistence;
using EnglishJourney.Infrastructure.Repositories;
using EnglishJourney.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
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
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<EnglishJourneyUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<EnglishJourneyDbContext>();

            services.AddScoped<IEnglishJourneySeeder, EnglishJourneySeeder>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IConnectionRepository, ConnectionRepository>();
            services.AddScoped<IFlashcardRepository, FlashcardRepository>();

            services.AddScoped<IEnglishJourneyAuthorizationService, EnglishJourneyAuthorizationService>();
        }
    }
}