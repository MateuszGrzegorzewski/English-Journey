using EnglishJourney.Application.Extensions;
using EnglishJourney.Infrastructure.Extensions;
using EnglishJourney.API.Extensions;
using EnglishJourney.API.Middlewares;
using Serilog;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Infrastructure.Seeders;
using Hangfire;
using EnglishJourney.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IEnglishJourneySeeder>();

await seeder.Seed();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<IUserStatisticsService>(
    "GetUserStatisticAsync",
    service => service.GetUserStatisticAsync(),
    Cron.Daily
    );

app.Run();

public partial class Program
{ }