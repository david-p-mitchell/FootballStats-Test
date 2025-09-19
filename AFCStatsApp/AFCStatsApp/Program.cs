using AFCStatsApp.Db;
using AFCStatsApp.Interfaces.Repositories;
using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using AFCStatsApp.Repositories;
using AFCStatsApp.Services;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FootballDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

builder.Services.Configure<FootballDataOrgApiSettings>(
    builder.Configuration.GetSection("FootballApi")
);

// Read settings to configure Refit client
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
