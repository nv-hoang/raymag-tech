using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using RefineCMS.Common;
using RefineCMS.Extensions;
using RefineCMS.Hooks;
using RefineCMS.Providers;

var builder = WebApplication.CreateBuilder(args);

// =================================================
// Add services to DI container
var services = builder.Services;
var config = builder.Configuration;

services.AddDbContext<DB>(options => options.UseSqlServer(config.GetConnectionString("DatabaseConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
services.AddMvc();

services.AddScoped<AppFactory>();

// configure DI for application providers
var cfg = new CFG(config.GetSection("AppConfig"));
services.AddSingleton(cfg);

// Configure Kestrel
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Limits.MaxRequestBodySize = cfg.MaxUploadSizeInMB * 1024 * 1024;
//});

// Add session services
services.AddDistributedMemoryCache(); // In-memory cache for session storage
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(cfg.SessionTimeoutInMinutes); // Set timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add localization services
services.AddLocalization(options => options.ResourcesPath = "Languages");
services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

services.AddControllers(options =>
{
    options.Conventions.Add(new AdminPrefixConvention(cfg.AdminSlug));
    options.Filters.Add<ControllerHookFilter>();
});

services.AddScoped<Page>();
services.AddScoped<Output>();

// Register hooks
services.AddSingleton(new HookManager(services));
services.AddScoped<HookProvider>();

// Providers
services.AddScoped(typeof(ModelProvider<>));
services.AddScoped<StorageProvider>();
services.AddScoped<AppOptions>();
// =================================================

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var _db = scope.ServiceProvider.GetRequiredService<DB>();
    _db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error"); // HomeController --> Error
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == 404 && !context.HttpContext.Response.HasStarted)
    {
        context.HttpContext.Response.Redirect("/error"); // HomeController --> Error
    }
    return Task.CompletedTask;
});

// Configure supported cultures
var supportedCultures = cfg.Languages.Select(x => x.Value).ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(cfg.DefaultLanguage)
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

localizationOptions.RequestCultureProviders = [
    new QueryStringCultureProvider(cfg.Languages.ToDictionary(x => x.Key, x => x.Value)),
    new CookieRequestCultureProvider(),
];

app.UseRequestLocalization(localizationOptions);

app.UseSession();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
