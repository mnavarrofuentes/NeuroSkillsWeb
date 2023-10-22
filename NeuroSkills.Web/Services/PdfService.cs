using DinkToPdf;
using DinkToPdf.Contracts;

namespace NeuroSkills.Web.Services;
public class PdfService : IPdfService
{
    private IConverter _converter;

    public PdfService(IConverter converter)
    {
        _converter = converter;
    }

    public byte[] GeneratePdfFromHtml(string htmlContent)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                }
            }
        };

        return _converter.Convert(doc);
    }
}
