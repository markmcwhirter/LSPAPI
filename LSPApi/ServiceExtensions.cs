using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;
using System;
using System.Collections.ObjectModel;
using System.Data;


namespace LSPApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        var logConfig = new LoggerConfiguration();


        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}  {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        logConfig.WriteTo.File(AppContext.BaseDirectory + $"/Logs\\Log-.log", outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day);
        logConfig.Enrich.FromLogContext();
        services.AddLogging(configure => configure.AddSerilog(logConfig.CreateLogger(), dispose: true));
        services.AddSingleton<Microsoft.Extensions.Logging.ILogger>((x) => x.GetRequiredService<ILogger<DefaultLogSource>>());


        return services;

    }
}




