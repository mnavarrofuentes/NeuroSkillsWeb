#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Models.Infrastructure;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;

namespace NeuroSkills.Web.Services;

public class ApplicationUserService : IApplicationUserService
{
    private static readonly string cacheKey = $"{typeof(ApplicationUserService)}.{nameof(ApplicationUserService)}";

    private readonly string endpoint;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMemoryCache memoryCache;
    private readonly ILogger<ApplicationUserService> logger;
    private readonly HttpClient httpClient;

    public ApplicationUserService(string endpoint,
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache,
        ILogger<ApplicationUserService> logger,
        HttpClient httpClient)
    {
        this.endpoint = $"{endpoint}/";
        this.httpContextAccessor = httpContextAccessor;
        this.memoryCache = memoryCache;
        this.logger = logger;
        this.httpClient = httpClient;
    }

    public async Task<ApplicationUser> LogIn(string userName, string password)
    {
        httpClient.BaseAddress = new Uri(endpoint);

        try
        {
            var response = await httpClient.PostAsJsonAsync("authentication/user-token", new { userName, password });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserTokenResponse>(content);

                if (!string.IsNullOrEmpty(data.Token))
                {
                    var applicationUser = GetApplicationUserFromJson(data.Token, data);
                    httpContextAccessor.HttpContext.Session.SetString(ApplicationUserMiddleware.TokenKey, data.Token);
                    httpContextAccessor.HttpContext.User = applicationUser;
                    memoryCache.Set($"{cacheKey}.{data.Token}", applicationUser);

                    return applicationUser;
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Se produjo un error inesperado mientras se realizaba el inicio de sesión");
        }

        return ApplicationUser.None;
    }

    public async Task LogOut()
    {
        var user = httpContextAccessor.HttpContext?.User as ApplicationUser ?? ApplicationUser.None;
        var token = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

        if (!string.IsNullOrEmpty(token))
        {
            memoryCache.Remove($"{cacheKey}.{token}");

            httpClient.BaseAddress = new Uri(endpoint);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            try
            {
                var response = await httpClient.PostAsync("authentication/user-token/revoke", null);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"Se produjo un error inesperado al realizar el cierre de sesión. HTTP Error: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Se produjo un error inesperado mientras se realizaba el cierre de sesión");
            }
        }

        httpContextAccessor.HttpContext.Session.Clear();
        httpContextAccessor.HttpContext.User = ApplicationUser.None;
    }

    public Task<ApplicationUser> GetApplicationUserByTokenAsync(string token)
    {
        var key = $"{cacheKey}.{token}";

        if (memoryCache.TryGetValue<ApplicationUser>(key, out var applicationUser))
            return Task.FromResult(applicationUser);
        else
            return Task.FromResult(ApplicationUser.None);
    }

    private ApplicationUser GetApplicationUserFromJson(string token, UserTokenResponse user)
    {
        if (user == null)
        {
            return ApplicationUser.None;
        }

        var settings = new Dictionary<string, object>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, token),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ApplicationUser(user.Id, user.UserName, identity, settings);
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.