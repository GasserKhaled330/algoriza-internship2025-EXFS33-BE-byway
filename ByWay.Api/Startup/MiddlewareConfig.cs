using ByWay.Api.Middleware;

namespace ByWay.Api.Startup;

public static class MiddlewareConfig
{
    public static void UseException(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}