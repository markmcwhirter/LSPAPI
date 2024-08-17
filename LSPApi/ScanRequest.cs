namespace LSPApi;

using LSPApi.Controllers;

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class ScanRequest
{
    private readonly RequestDelegate _next;

    public ScanRequest(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Logic to execute before the next middleware/request handler in the pipeline
        // Define the URL pattern you want to enforce
        string[] requiredPatterns = { "author","book", "sale", "user", "help", "vendor" }; 

        // Get the requested path from the HttpContext
        string requestedPath = context.Request.Path;        

        bool match = false;
        foreach( var r in requiredPatterns)
        {
            if (requestedPath.StartsWith("/api/" + r, StringComparison.OrdinalIgnoreCase))
            {
                match = true; 
                break;
            }

        }
        //context.Response.OnStarting(() =>
        //{
        //    context.Response.Headers.Remove("Server");
        //    return Task.CompletedTask;
        //});

        if (!match)
        {
            // URL doesn't match the required pattern, raise an error
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Access denied. URL pattern mismatch.");

            return; // Stop further processing
        }



        await _next(context); // Call the next middleware/request handler

        // Logic to execute after the next middleware/request handler has completed
        ;
    }
}

