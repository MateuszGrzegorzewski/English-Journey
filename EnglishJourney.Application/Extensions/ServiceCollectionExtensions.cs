using EnglishJourney.Application.Mappings;
using EnglishJourney.Application.Note.Commands.CreateNote;
using EnglishJourney.Application.Note.Query.GetAllNotes;
using EnglishJourney.Application.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishJourney.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetAllNotesQuery)));

            services.AddAutoMapper(typeof(NoteMappingProfile));
            services.AddAutoMapper(typeof(ConnectionMappingProfile));
            services.AddAutoMapper(typeof(FlashcardMappingProfile));

            services.AddValidatorsFromAssemblyContaining<CreateNoteCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddScoped<IUserContext, UserContext>();

            services.AddHttpContextAccessor();
        }
    }
}