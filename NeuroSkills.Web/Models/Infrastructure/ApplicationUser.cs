using NeuroSkills.Web.Infrastructure;
using System.Security.Claims;
using System.Security.Principal;

namespace NeuroSkills.Web.Models.Infrastructure;

public class ApplicationUser : ClaimsPrincipal
{
    public static ApplicationUser None { get; } = new ApplicationUser { Id = -1, UserName = "Anónimo" };

    public int Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public Dictionary<string, object> Settings { get; private set; }
    public bool IsAuthenticated => this.Id != None.Id;

    private ApplicationUser()
    {
        Settings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationUser(int id, string userName, IIdentity identity, IDictionary<string, object> settings)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(identity)
    {
        Id = id;
        UserName = userName;
        settings = settings
            .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
    }

    public bool HasRole(string name) => HasClaim(RoleAttribute.ClaimType, name);
}