namespace NeuroSkills.Web.Models.Infrastructure;

public class UserTokenResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
