using Microsoft.AspNetCore.Mvc;
using NeuroSkills.Web.Extensions;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models;
using NeuroSkills.Web.Models.Views;
using NeuroSkills.Web.Services;

namespace NeuroSkills.Web.Controllers;
public class ReportController : Controller
{
    private readonly IPdfService pdfService;

    public ReportController(IPdfService pdfService)
    {
        this.pdfService = pdfService;
    }

    [HttpGet]
    [Role(Constants.AdminRole)]
    [Role(Constants.TutorRole)]
    [Role(Constants.StudentRole)]
    public IActionResult StudentResult()
    {
        return View();
    }

    [HttpPost]
    [Role(Constants.AdminRole)]
    [Role(Constants.TutorRole)]
    [Role(Constants.StudentRole)]
    public async Task<IActionResult> StudentResult([FromBody]StudentResultViewModel model)
    {
        try
        {
            var html = await this.RenderViewAsync("_StudentResultReport", model);

            // Generate the PDF
            var pdfBytes = pdfService.GeneratePdfFromHtml(html);

            // Return the PDF as a file download
            return File(pdfBytes, "application/pdf");
        }
        catch (Exception e)
        {
            e.ToString();
            throw;
        }
    }
}
