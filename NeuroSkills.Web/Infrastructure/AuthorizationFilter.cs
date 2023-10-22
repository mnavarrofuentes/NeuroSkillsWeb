using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NeuroSkills.Web.Models;
using NeuroSkills.Web.Models.Infrastructure;
using System.Net;

namespace NeuroSkills.Web.Infrastructure;

public class AuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var controller = (string)context.RouteData.Values["controller"];
        var action = (string)context.RouteData.Values["action"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        var user = context.HttpContext.User as ApplicationUser ?? ApplicationUser.None;
        var returnUrl = context.HttpContext.Request.Path + (context.HttpContext.Request.QueryString.HasValue
            ? context.HttpContext.Request.QueryString
            : string.Empty);

        // Redirect to the /Login page if the user is not auth and they are not already trying to hit the /Login page
        if (!user.IsAuthenticated)
        {
            if (!(controller == "Home" && action == "Login"))
                context.Result = new RedirectToActionResult("Login", "Home", new { returnUrl });
        }
        else
        {
            var roles = context.ActionDescriptor.EndpointMetadata
                .Where(m => m is RoleAttribute)
                .Cast<RoleAttribute>();

            if (roles.Any() && !roles.Any(p => user.HasRole(p.Name)))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                if (context.HttpContext.Request.IsAjaxRequest())
                    context.Result = new JsonResult("You do not have permission to access this resource.");
                else
                {
                    var model = new ErrorViewModel { RequestId = context.HttpContext.TraceIdentifier };
                    var vdd = new ViewDataDictionary<ErrorViewModel>(new EmptyModelMetadataProvider(), context.ModelState) { Model = model };

                    context.Result = new ViewResult
                    {
                        ViewName = "Forbidden",
                        ViewData = vdd
                    };
                }
            }
        }
    }
}
