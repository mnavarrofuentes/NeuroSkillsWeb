using Microsoft.AspNetCore.Mvc;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models;

namespace NeuroSkills.Web.Controllers;
public class TherapyController : Controller
{
    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Modules(int id)
    {
        return View(id);
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Schedules(int id)
    {
        return View(id);
    }
}
