namespace LearningJourney.API.Controllers
{

    public class AttachmentsController(ISender mediator) : BaseController(mediator)
    {
        [MultipartFormData]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(1000 * 1024 * 1024)] // 100MB
        [RequestFormLimits(
            MultipartBodyLengthLimit = 1000 * 1024 * 1024       // 1GB total request size
                                                                //MultipartHeadersLengthLimit = 16384,              // 16KB headers limit
                                                                //MultipartBoundaryLengthLimit = 128,               // Boundary string length
                                                                //ValueLengthLimit = 4194304,                       // 4MB per form value
                                                                //ValueCountLimit = 1024,                           // Max number of form values
                                                                //BufferBodyLengthLimit = 134217728,                // 128MB buffer limit
                                                                //BufferBody = true,                                // Buffer the body
                                                                //Order = 0                                         // Filter order
        )]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadFilesCommand command, CancellationToken cancellationToken)
        {

            var fileNames = await _mediator.Send(command, cancellationToken);
            return Ok(fileNames);
        }
        [HttpPost("export")]
        public IActionResult Export([FromBody] ExportFilesRequest request, [FromServices] IBackgroundTaskQueue taskQueue)
        {
            var exportJobId = Guid.NewGuid().ToString();

            taskQueue.QueueBackgroundWorkItem((Func<CancellationToken, Task>)(async ct =>
            {
                await _mediator.Send(new ExportFilesCommand(request.FileNames, exportJobId), ct);
            }));

            return Accepted(new { Message = "Export queued", JobId = exportJobId });
        }
    }
}
