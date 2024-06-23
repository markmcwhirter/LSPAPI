/*

using Serilog;


namespace LSPApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        var logConfig = new LoggerConfiguration();


        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}  {Level:u3}] ({Application}/{MachineName}/{ThreadId})  {SourceContext}: {Message:lj}{NewLine}{Exception}";

        logConfig.Enrich.FromLogContext();
        logConfig.Enrich.WithEnvironmentUserName();

        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build();

        services.AddLogging(l =>
        {
            l.AddSeq("http://209.38.64.145:5341");
            l.AddConsole();
            l.AddFile("logs/log.txt", outputTemplate: outputTemplate);
            l.AddConfiguration(configuration);            
        });
        services.AddSingleton<Microsoft.Extensions.Logging.ILogger>((x) => x.GetRequiredService<ILogger<DefaultLogSource>>());


        return services;

    }
}
*/
/*
 builder.Host.UseSerilog((ctx, lc) => lc
.WriteTo.File("logs/log-.txt",
    rollingInterval: RollingInterval.Day,
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
.MinimumLevel.Debug()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
// Filter out ASP.NET Core infrastructre logs that are Information and below
.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
.Enrich.FromLogContext()
.WriteTo.Seq("http://209.38.64.145:5341")
.Enrich.WithProperty("Application", "LSP3")
.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
);
 */