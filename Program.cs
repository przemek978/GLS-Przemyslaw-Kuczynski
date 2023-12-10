using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using Triggers.Data;
using Triggers.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DBContext>(options =>
                    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LabelsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"), ServiceLifetime.Transient);

        services.AddScoped<IDataService, DataService>();
    })
    .Build();
host.Run();
