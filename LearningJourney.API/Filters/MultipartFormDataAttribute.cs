namespace LearningJourney.API.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MultipartFormDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (request.HasFormContentType &&
                request.ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                // Additional validation can go here
                // e.g., check boundary, size limits, etc.
                return;
            }

            // Note: This shouldn't normally execute if using IActionConstraint
            // but provides extra safety
            context.ModelState.AddModelError("ContentType",
                "Request must be multipart/form-data");

            context.Result = new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType);
        }
    }
}