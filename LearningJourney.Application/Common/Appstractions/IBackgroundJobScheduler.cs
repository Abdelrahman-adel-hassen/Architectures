namespace LearningJourney.Application.Common.Appstractions
{
    public interface IBackgroundJobScheduler
    {
        // Fire & forget
        string Enqueue<TJob>(Expression<Func<TJob, Task>> method);

        // Delayed
        string Schedule<TJob>(
            Expression<Func<TJob, Task>> method,
            TimeSpan delay);

        // Scheduled at exact date
        string Schedule<TJob>(
            Expression<Func<TJob, Task>> method,
            DateTimeOffset runAt);

        // Recurring (weekly / monthly / cron)
        void AddOrUpdateRecurring<TJob>(
            string jobId,
            Expression<Func<TJob, Task>> method,
            string cron,
            TimeZoneInfo? timeZone = null);

        // Remove recurring
        void RemoveRecurring(string jobId);

        // Trigger manually
        void TriggerRecurring(string jobId);
    }
}
