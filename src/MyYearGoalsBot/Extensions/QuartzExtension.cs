using MyYearGoalsBot.App.Options;
using MyYearGoalsBot.Jobs;
using Quartz;

namespace MyYearGoalsBot.Extensions;

public static class QuartzExtension
{
    /// <summary>
    /// Расширение для регистрации заданий.
    /// </summary>
    public static void AddQuartzJobs(this IServiceCollection services, QuartzAppOptions quartzOptions)
    {
        services
            .AddQuartz(quartzConfigurator =>
            {
                quartzConfigurator.UseMicrosoftDependencyInjectionJobFactory();
                quartzConfigurator.AddJob<NotifyGoalsJob>(jobConfigurator =>
                {
                    jobConfigurator.WithIdentity(NotifyGoalsJob.Key, "app");
                });
                quartzConfigurator.AddTrigger(triggerConfigurator =>
                {
                    triggerConfigurator.ForJob(NotifyGoalsJob.Key, "app")
                        .WithIdentity(NotifyGoalsJob.TriggerKey, "app")
                        .WithCronSchedule(quartzOptions.GoalsNotificationJobCron);
                        //.StartNow(); // Для теста
                });
            });
            

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
    }
}