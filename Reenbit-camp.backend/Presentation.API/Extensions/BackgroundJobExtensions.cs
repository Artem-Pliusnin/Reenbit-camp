using Hangfire;
using Infrastructure.BackgroundJobs;

namespace Presentation.API.Extensions;

public static class BackgroundJobExtensions
{
    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<ArchivePendingBoardsJob>(
                "archive-pending-boards",
                job => job.ExecuteAsync(),
                app.Configuration["BackgroundJobs:Archiving:Schedule"]);
        
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<CancelExpiredSubscriptionsJob>(
                "cancel-expired-subscriptions",
                job => job.ExecuteAsync(),
                app.Configuration["BackgroundJobs:CancelingSubscriptions:Schedule"]);

        return app;
    }
}