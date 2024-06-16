﻿
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

        services.AddLogger();

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
