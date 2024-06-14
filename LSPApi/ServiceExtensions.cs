using Serilog;


namespace LSPApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        var logConfig = new LoggerConfiguration();


        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}  {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        //.WriteTo.File(AppContext.BaseDirectory + $"Log-.log", outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day);

        logConfig.Enrich.FromLogContext();
        logConfig.Enrich.WithEnvironmentUserName();

        services.AddLogging(l =>
        {
            l.AddSeq("http://209.38.64.145:5341");
            l.AddConsole();
            l.AddFile("logs/log-.txt", outputTemplate: outputTemplate);
        });
        services.AddSingleton<Microsoft.Extensions.Logging.ILogger>((x) => x.GetRequiredService<ILogger<DefaultLogSource>>());


        return services;

    }
}


/*
 * "logs/log-.txt"
         services.AddLogging(l =>
        {
            l.AddSeq("http://209.38.64.145:5341");
            l.AddConsole();
            l.AddFile("logs/log-.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"); 
        });
 */

