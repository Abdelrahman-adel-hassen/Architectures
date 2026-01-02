namespace LearningJourney.Infrastructure.BackgroundJobs
{
    public class FileExportBackgroundService(IBackgroundTaskQueue queue, ILogger<FileExportBackgroundService> logger) : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue = queue;
        private readonly ILogger<FileExportBackgroundService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var workItem = await _queue.DequeueAsync(stoppingToken);
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background export failed");
                }
            }
        }
    }


}
