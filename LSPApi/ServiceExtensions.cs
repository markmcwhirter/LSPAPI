using Serilog;


namespace LSPApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        var logConfig = new LoggerConfiguration();


        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}  {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        logConfig.WriteTo.File(AppContext.BaseDirectory + $"Log-.log", outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day);
        logConfig.Enrich.FromLogContext();
        services.AddLogging(configure => configure.AddSerilog(logConfig.CreateLogger(), dispose: true));
        services.AddSingleton<Microsoft.Extensions.Logging.ILogger>((x) => x.GetRequiredService<ILogger<DefaultLogSource>>());


        return services;

    }
}




