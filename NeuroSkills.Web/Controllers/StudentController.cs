using Microsoft.AspNetCore.Mvc;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models;

namespace NeuroSkills.Web.Controllers;
public class StudentController : Controller
{
    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Contacts(int id)
    {
        return View(id);
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Therapies(int id)
    {
        return View(id);
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult TherapyModules(int id)
    {
        return View(id);
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult TherapySchedules(int id)
    {
        return View(id);
    }
}
