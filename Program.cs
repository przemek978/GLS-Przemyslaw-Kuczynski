using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        //services.AddDbContext<DBContext>(options =>
        //            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PackagesDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"), ServiceLifetime.Transient);
        
        //using (var scope = services.BuildServiceProvider().CreateScope())
        //{
        //    var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
        //    dbContext.Database.EnsureCreated();
        //}
        //services.AddScoped<IGLSAPI,GLSAPI>();
        //services.AddScoped<IGLSService, GLSService>();
        //services.AddScoped<IGLSRepository, GLSRepository>();
    })
    .Build();
host.Run();
