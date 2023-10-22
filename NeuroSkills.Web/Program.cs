using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.Extensions.Caching.Memory;
using NeuroSkills.Web.Infrastructure;
using NeuroSkills.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IApplicationUserService>(p =>
{
    var httpClient = p.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new ApplicationUserService(
            builder.Configuration["Api"],
            p.GetRequiredService<IHttpContextAccessor>(),
            p.GetRequiredService<IMemoryCache>(),
            p.GetRequiredService<ILogger<ApplicationUserService>>(),
            httpClient
        );
});
builder.Services.AddScoped<IPdfService, PdfService>();

builder.Services.AddSession(o => o.IdleTimeout = TimeSpan.MaxValue);

builder.Services.AddMvc().AddControllersAsServices();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AuthorizationFilter>();
    options.Filters.Add<UnhandledExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
    app.UseDeveloperExceptionPage();

app.UseStatusCodePagesWithReExecute("/Home/Error", "code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.GetApplicationUserBySessionId();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
