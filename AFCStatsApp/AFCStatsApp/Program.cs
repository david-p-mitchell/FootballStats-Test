using AFCStatsApp.Db;
using AFCStatsApp.Interfaces.Repositories;
using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using AFCStatsApp.Repositories;
using AFCStatsApp.Services;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using React.AspNet;
using Refit;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FootballDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

builder.Services.Configure<FootballDataOrgApiSettings>(
    builder.Configuration.GetSection("FootballApi")
);

builder.Services.AddReact();

builder.Services.AddJsEngineSwitcher(options =>
{
    options.DefaultEngineName = ChakraCoreJsEngine.EngineName;
}).AddChakraCore();

var apiSettings = builder.Configuration.GetSection("FootballDataOrgApi").Get<FootballDataOrgApiSettings>();



builder.Services.AddRefitClient<IMatchesAPI>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(apiSettings!.BaseUrl);
        c.DefaultRequestHeaders.Add("X-Auth-Token", apiSettings.ApiKey);
    });

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddControllersWithViews();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5132);
    options.ListenAnyIP(7267, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseReact(config =>
{
    // Optional: configure Babel/JSX transforms or add scripts
    config
        .AddScript("~/js/playerModal/playerModal.jsx");
});
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
