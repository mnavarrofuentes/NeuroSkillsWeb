using NeuroSkills.Web.Models.Infrastructure;
using NeuroSkills.Web.Services;

namespace NeuroSkills.Web.Infrastructure;

public class ApplicationUserMiddleware
{
    public static readonly string TokenKey = "x-api-token";
    private readonly RequestDelegate next;

    public ApplicationUserMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, IApplicationUserService applicationUserService)
    {
        var token = context.Session.GetString(TokenKey);

        if (!string.IsNullOrEmpty(token))
        {
            context.User = await applicationUserService.GetApplicationUserByTokenAsync(token);
        }
        else if (context.User is not ApplicationUser)
        {
            context.User = ApplicationUser.None;
        }

        await next(context);
    }
}

public static class ApplicationUserMiddlewareExtension
{
    public static IApplicationBuilder GetApplicationUserBySessionId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApplicationUserMiddleware>();
    }
}