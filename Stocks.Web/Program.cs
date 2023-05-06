using Serilog;
using Stocks.Web.Middlewares;
using Stocks.Web.StartupExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stocks.Infrastructure.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsEnvironment("Test"))
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", "Rotativa");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
await SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists=User}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();

async Task SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        // TODO: Initialize db server with default roles and admin user
    }
}
public partial class Program { }