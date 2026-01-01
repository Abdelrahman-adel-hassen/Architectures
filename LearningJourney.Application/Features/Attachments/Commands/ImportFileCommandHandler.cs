namespace LearningJourney.Application.Features.Attachments.Commands;

public record UploadFilesCommand(List<IFormFile> Files) : IRequest<List<string>>;


public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, List<string>>
{
    public async Task<List<string>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        const long maxFileSize = 1000 * 1024 * 1024;
        var fileNames = new List<string>();

        foreach (var file in request.Files)
        {
            var fileName = await ProcessSingleFile(file, maxFileSize, cancellationToken);
            fileNames.Add(fileName);
        }

        return fileNames;
    }
    private static async Task<string> ProcessSingleFile(IFormFile file, long maxFileSize, CancellationToken cancellationToken)
    {
        ValidateFile(file, maxFileSize);

        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(uploadsPath);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return fileName;
    }

    private static void ValidateFile(IFormFile file, long maxFileSize)
    {
        var allowedTypes = new[]
        {
                "image/jpeg",
                "image/png",
                "image/jpg",
                "image/webp",
                "application/pdf"
            };

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".pdf" };

        if (!allowedTypes.Contains(file.ContentType))
            throw new BadRequestException("Only images and PDF files are allowed");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            throw new BadRequestException("Invalid file extension");

        if (file.Length > maxFileSize)
            throw new BadRequestException($"File size must be less than {maxFileSize / (1024 * 1024)} MB");
    }
}