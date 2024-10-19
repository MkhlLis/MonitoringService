using System.Reflection;
using MonitoringService.Contracts.Interfaces;
using MonitoringService.Handlers;
using MonitoringService.Services;
using MonitoringService.Store;

namespace MonitoringService;

public class Startup
{
    public static WebApplication InitializeApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();
        Configure(app);
        return app;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        Configure(builder.Services);
    }

    private static void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static IServiceCollection Configure(IServiceCollection services)
    {
        services.AddScoped<IMonitoringHandler, MonitoringHandler>();
        services.AddSingleton<IStore, InMemoryProductStore>();

        services.AddHostedService<AvailableCheckerService>();
        return services;
    }
}