#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8603 // Possible null reference return.
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using System.Security.Claims;
using System.Text.Json;

namespace NeuroSkills.Web.Models.Infrastructure;

public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
{
    [RazorInject]
    public IConfiguration Configuration { get; set; }
    public new ApplicationUser User => base.User as ApplicationUser;
    public string Token => User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
    public string Api => Configuration["Api"];
    public readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8603 // Possible null reference return.
