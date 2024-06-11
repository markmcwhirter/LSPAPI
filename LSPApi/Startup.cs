
using LSPApi.DataLayer;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

using Serilog;

namespace LSPApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("LSPConnection");

        services.AddLogging(l =>
        {
            l.AddSeq("http://209.38.64.145:5341");
            l.AddConsole();
            l.AddFile("logs/log-.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"); 
        });

        services.AddMemoryCache();

 
        services.AddDbContext<LSPContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();


        services.AddCors(options =>
        {
            options.AddPolicy("corspolicy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

    }



    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("corspolicy");
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // using Microsoft.AspNetCore.HttpOverrides;

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
