using Microsoft.AspNetCore.Mvc;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models;

namespace NeuroSkills.Web.Controllers;
public class TutorController : Controller
{
    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Role(Constants.TutorRole)]
    public IActionResult Assignations()
    {
        return View();
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    [Role(Constants.TutorRole)]
    public IActionResult StudentSessions()
    {
        return View();
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    [Role(Constants.TutorRole)]
    public IActionResult StudentResults()
    {
        return View();
    }
}
