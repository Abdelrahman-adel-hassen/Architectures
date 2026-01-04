using System.Security.Claims;
using LearningJourney.Application.Common.Appstractions;
using LearningJourney.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningJourney.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value; // This will contain IdNumber from JWT

    public UserType? UserType
    {
        get
        {
            var userTypeClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserType")?.Value;
            return Enum.TryParse<UserType>(userTypeClaim, out var userType) ? userType : null;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
