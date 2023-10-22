namespace NeuroSkills.Web.Services;

public interface IPdfService
{
    byte[] GeneratePdfFromHtml(string htmlContent);
}
