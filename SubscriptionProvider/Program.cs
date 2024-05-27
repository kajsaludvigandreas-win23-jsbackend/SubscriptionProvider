using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubscriptionProvider.Data.Contexts;
using System.Diagnostics;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("SQLDATABASE")));
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var migration = context.Database.GetPendingMigrations();
        if ((migration != null && migration.Any()))
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"ERROR : SubscriptionProvider.Program.cs :: {ex.Message}");
    }

}

host.Run();
