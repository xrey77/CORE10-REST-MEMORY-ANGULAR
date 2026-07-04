// // Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .WithMethods("GET", "POST", "DELETE", "PATCH", "OPTIONS")
              .WithExposedHeaders("Content-Disposition")
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

var app = builder.Build();

// Enable serving files from wwwroot
app.UseDefaultFiles(); // Searches for index.html automatically
app.UseStaticFiles();

// Apply CORS before routing
app.UseCors(); 
app.UseRouting();

app.MapControllers();

// If no API or static file matches, fall back to index.html for SPA client-side routing
app.MapFallbackToFile("{*path:nonfile}", "index.html");

app.Run();

public partial class Program { }

