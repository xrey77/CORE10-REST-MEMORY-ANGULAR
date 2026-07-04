// Program.cs
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
              .WithMethods("GET", "POST","DELETE","PATCH","POST","OPTIONS")
              .WithExposedHeaders("Content-Disposition")
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

var app = builder.Build();

// 1. MUST come before UseStaticFiles to intercept requests for the root directory (/)
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new[] { "index.html" }
});

// 2. Serves the static index.html file and other assets (css, js, images) from wwwroot
app.UseStaticFiles(); 
app.UseCors( options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseRouting();

// app.UseAuthorization(); // If you use authentication/authorization, ensure it's placed here

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
