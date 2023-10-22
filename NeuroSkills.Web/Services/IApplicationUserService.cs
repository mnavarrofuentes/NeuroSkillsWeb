using NeuroSkills.Web.Models.Infrastructure;

namespace NeuroSkills.Web.Services;

public interface IApplicationUserService
{
    Task<ApplicationUser> LogIn(string userName, string password);
    Task LogOut();
    Task<ApplicationUser> GetApplicationUserByTokenAsync(string sid);
}
