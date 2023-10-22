namespace NeuroSkills.Web.Models;

public class LoginViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
}
