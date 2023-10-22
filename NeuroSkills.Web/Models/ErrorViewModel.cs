namespace NeuroSkills.Web.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public Exception? Exception { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
