using System.Security.Claims;

/// <summary>
/// Middleware to identify requests originating from localhost and add "Administrator" role to claims if not authenticated.
/// </summary>
public class LocalhostMiddleware
{
    private readonly RequestDelegate _next;

    public LocalhostMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var isLocalhost = context.Request.Host.Host == "localhost" || context.Request.Host.Host == "127.0.0.1";
        context.Items["IsLocalhost"] = isLocalhost;

        if (isLocalhost)
        {
            // If request is from localhost, add "Administrator" role to claims
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null && !claimsIdentity.IsAuthenticated)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
            }
        }

        await _next(context);
    }
}
public static class LocalhostMiddlewareExtensions
{
    public static IApplicationBuilder UseLocalhostMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LocalhostMiddleware>();
    }
}