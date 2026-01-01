namespace LearningJourney.Application.Features.Attachments.Commands
{
    public record ExportFilesCommand(List<string> FileNames, string ExportJobId) : IRequest<Unit>;
    public class ExportFilesRequest
    {
        [System.ComponentModel.DataAnnotations.Required]
        public List<string> FileNames { get; set; }
    }

    public class ExportFilesCommandHandler : IRequestHandler<ExportFilesCommand, Unit>
    {
        public async Task<Unit> Handle(ExportFilesCommand request, CancellationToken cancellationToken)
        {
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            
            ValidateSizeLimit(request, uploadsDir);

            var exportDir = Path.Combine(Directory.GetCurrentDirectory(), "Exports");
            Directory.CreateDirectory(exportDir);



            var exportFilePath = Path.Combine(exportDir, $"{request.ExportJobId}.zip");


            using var zip = new System.IO.Compression.ZipArchive(
                new FileStream(exportFilePath, FileMode.Create),
                System.IO.Compression.ZipArchiveMode.Create
            );

            foreach (var fileName in request.FileNames)
            {
                var filePath = Path.Combine(uploadsDir, fileName);
                if (!File.Exists(filePath)) continue;

                var entry = zip.CreateEntry(fileName, System.IO.Compression.CompressionLevel.Fastest);
                await using var entryStream = entry.Open();
                await using var fileStream = File.OpenRead(filePath);
                await fileStream.CopyToAsync(entryStream, cancellationToken);
            }

            return Unit.Value;
        }

        private static void ValidateSizeLimit(ExportFilesCommand request, string uploadsDir)
        {
            const long maxTotalSize = 1000 * 1024 * 1024; // 500 MB
            long totalSize = 0;

            foreach (var fileName in request.FileNames)
            {
                var filePath = Path.Combine(uploadsDir, fileName);
                if (!File.Exists(filePath)) continue;

                var fileInfo = new FileInfo(filePath);
                totalSize += fileInfo.Length;

                if (totalSize > maxTotalSize)
                    throw new InvalidOperationException(
                        $"Total export size exceeds the limit of {maxTotalSize / (1024 * 1024)} MB.");
            }
        }
    }

}
