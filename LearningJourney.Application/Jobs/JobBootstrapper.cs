namespace LearningJourney.Application.Jobs
{
    public static class JobBootstrapper
    {
        public static void RegisterJobs(IBackgroundJobScheduler scheduler)
        {
            scheduler.AddOrUpdateRecurring<AppointmentExportJob>(
                jobId: "appointments-export-weekly",
                method: job => job.ExportAppointmentsToExcel(),
                cron: "0 2 * * 0", // Sunday 2 AM
                timeZone: TimeZoneInfo.Utc
            );

            //scheduler.Enqueue<AppointmentExportJob>(method: job => job.ExportAppointmentsToExcel());

            // You can add more jobs here later
        }
    }
}
