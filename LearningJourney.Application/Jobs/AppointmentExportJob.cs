namespace LearningJourney.Application.Jobs
{
    public class AppointmentExportJob(IMediator mediator, IExcelExporter excelExporter, IOptions<ExportSettings> options)
    {
        private readonly IMediator _mediator = mediator;
        private readonly IExcelExporter _excelExporter = excelExporter;
        private readonly string _exportFolder = options.Value.ExportFolderPath;

        public async Task ExportAppointmentsToExcel()
        {
            var appointments = await _mediator.Send(new GetAppointmentsQuery());

            string fileName = $"Appointments_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string fullPath = Path.Combine(_exportFolder, fileName);

            await _excelExporter.ExportAsync(appointments, fullPath);

            Console.WriteLine($"Appointments exported to {fullPath}");
        }
    }
}
