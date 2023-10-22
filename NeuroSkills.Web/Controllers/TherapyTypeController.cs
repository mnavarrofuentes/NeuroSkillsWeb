using Microsoft.AspNetCore.Mvc;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models;

namespace NeuroSkills.Web.Controllers;
public class TherapyTypeController : Controller
{
    [HttpGet]
    [Role(Constants.AdminRole)]
    public IActionResult Index()
    {
        return View();
    }
}
