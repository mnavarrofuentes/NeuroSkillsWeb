using System.Security.Claims;

namespace NeuroSkills.Web.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class RoleAttribute : Attribute
{
    public RoleAttribute(string name)
    {
        Name = name;
    }

    public static string ClaimType => ClaimTypes.Role;
    public string Name { get; set; }
}
