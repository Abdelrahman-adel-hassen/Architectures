namespace LearningJourney.Infrastructure.Services
{
    public class ClosedXmlExcelExporter : IExcelExporter
    {
        public Task ExportAsync<T>(IEnumerable<T> data, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(typeof(T).Name + "s");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add headers with bold
            for (int i = 0; i < properties.Length; i++)
            {
                var headerCell = worksheet.Cell(1, i + 1);
                headerCell.SetValue(properties[i].Name);
                headerCell.Style.Font.Bold = true;
            }

            // Add data rows
            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item);
                    var cell = worksheet.Cell(row, col + 1);

                    switch (value)
                    {
                        case null:
                            cell.SetValue("");
                            break;
                        case string s:
                            cell.SetValue(s);
                            break;
                        case DateTime dt:
                            cell.SetValue(dt);
                            cell.Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                            break;
                        case bool b:
                            cell.SetValue(b ? "Yes" : "No");
                            break;
                        case int i:
                            cell.SetValue(i);
                            break;
                        case long l:
                            cell.SetValue(l);
                            break;
                        case float f:
                            cell.SetValue(f);
                            break;
                        case double d:
                            cell.SetValue(d);
                            break;
                        case decimal dec:
                            cell.SetValue(dec);
                            break;
                        default:
                            cell.SetValue(value.ToString());
                            break;
                    }
                }
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            workbook.SaveAs(filePath);

            return Task.CompletedTask;
        }
    }
}
