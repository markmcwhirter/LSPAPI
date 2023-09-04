using Serilog.Formatting.Compact;
using Serilog;
using Microsoft.AspNetCore.Hosting;

namespace LSPApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File(new RenderedCompactJsonFormatter(), "log.txt")
            .CreateLogger();


            var host = new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel()
            .ConfigureAppConfiguration((builder, config) =>
            {
                var env = builder.HostingEnvironment;

                config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            })
            .UseStartup<Startup>()
            .Build();



            host.Run();
        }

    }
}