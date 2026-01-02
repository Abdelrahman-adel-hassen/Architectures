namespace LearningJourney.Application.Common.Appstractions
{
    public interface IExcelExporter
    {
        Task ExportAsync<T>(IEnumerable<T> data, string filePath);
    }
}
