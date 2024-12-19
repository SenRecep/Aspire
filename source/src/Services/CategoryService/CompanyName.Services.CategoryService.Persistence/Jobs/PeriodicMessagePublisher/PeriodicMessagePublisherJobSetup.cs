using Microsoft.Extensions.Options;
using Quartz;

namespace CompanyName.Services.CategoryService.Persistence.Jobs.PeriodicMessagePublisher;

public class PeriodicMessagePublisherJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(PeriodicMessagePublisherJob));
        options
            .AddJob<PeriodicMessagePublisherJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(TimeSpan.FromSeconds(15)).RepeatForever()));
    }
}
