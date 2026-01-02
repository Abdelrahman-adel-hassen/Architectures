namespace LearningJourney.Infrastructure.BackgroundJobs
{
    public class HangfireJobScheduler : IBackgroundJobScheduler
    {
        public string Enqueue<TJob>(Expression<Func<TJob, Task>> method)
            => BackgroundJob.Enqueue(method);

        public string Schedule<TJob>(
            Expression<Func<TJob, Task>> method,
            TimeSpan delay)
            => BackgroundJob.Schedule(method, delay);

        public string Schedule<TJob>(
            Expression<Func<TJob, Task>> method,
            DateTimeOffset runAt)
            => BackgroundJob.Schedule(method, runAt);

        public void AddOrUpdateRecurring<TJob>(
            string jobId,
            Expression<Func<TJob, Task>> method,
            string cron,
            TimeZoneInfo? timeZone = null)
        {
            RecurringJob.AddOrUpdate(
                jobId,
                methodCall: method,
                cronExpression: cron,
                new RecurringJobOptions
                {
                    TimeZone = timeZone ?? TimeZoneInfo.Utc,
                }
            );
        }

        public void RemoveRecurring(string jobId)
            => RecurringJob.RemoveIfExists(jobId);

        public void TriggerRecurring(string jobId)
            => RecurringJob.TriggerJob(jobId);
    }
}
