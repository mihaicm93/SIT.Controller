public interface ILocalhostService
{
    bool IsLocalhost();
    bool IsLocalhostAdministrator();
}

public class LocalhostService : ILocalhostService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalhostService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsLocalhost()
    {
        var isLocalhost = _httpContextAccessor.HttpContext?.Items["IsLocalhost"] as bool?;
        return isLocalhost ?? false;
    }

    public bool IsLocalhostAdministrator()
    {
        if (IsLocalhost())
        {
            // For localhost, consider it as Administrator role
            return true;
        }
        return false;
    }
}
