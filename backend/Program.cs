using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

var frontendRoot = Path.Combine(app.Environment.ContentRootPath, "..", "app");
var frontendProvider = new PhysicalFileProvider(frontendRoot);

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = frontendProvider,
    DefaultFileNames = new[] { "index.html" }
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = frontendProvider,
    RequestPath = ""
});

app.MapGet("/api/hello", () => Results.Ok(new
{
    message = "Hello from GH200 backend!",
    timestamp = DateTime.UtcNow
}));

app.MapControllers();

app.Run();

// Exposes the entry point to WebApplicationFactory in the test project.
public partial class Program { }
