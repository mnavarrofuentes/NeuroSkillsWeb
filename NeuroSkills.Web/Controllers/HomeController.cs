using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models;
using NeuroSkills.Web.Services;
using System.Diagnostics;
using System.Net;

namespace NeuroSkills.Web.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApplicationUserService applicationUserService;

    public HomeController(ILogger<HomeController> logger,
        IApplicationUserService applicationUserService)
    {
        _logger = logger;
        this.applicationUserService = applicationUserService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult UserData()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        await applicationUserService.LogOut();
        var model = new LoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await applicationUserService.LogIn(model.UserName, model.Password);

        if (!user.IsAuthenticated)
        {
            model.Errors.Add("Usuario o password incorrrecto.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await applicationUserService.LogOut();

        return RedirectToAction(nameof(Login));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(HttpStatusCode code = default)
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error ?? new Exception("Un error ha ocurrido. Por favor contacta al administrado si el problema persiste.");
        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Exception = exception
        };

        if (HttpContext.Request.IsAjaxRequest())
            return Json(model);
        else
        {
            return (code) switch
            {
                HttpStatusCode.Unauthorized => RedirectToAction(nameof(Login)),
                HttpStatusCode.Forbidden => View("Forbidden", model),
                HttpStatusCode.NotFound => View("NotFound", model),
                _ => View(model)
            };
        }
    }

    [HttpGet("/Forbidden")]
    public IActionResult Forbidden_() => PartialView("Forbidden", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [HttpGet("/NotFound")]
    public IActionResult NotFound_() => PartialView("NotFound", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
