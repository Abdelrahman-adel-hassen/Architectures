namespace LearningJourney.API.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AdminOnlyAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

        if (!currentUserService.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (currentUserService.UserType != UserType.Admin)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
