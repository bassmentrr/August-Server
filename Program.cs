var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://127.0.0.1:6000");
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(o => o.SingleLine = true);
var app = builder.Build();

string contentDir = Path.Combine(app.Environment.ContentRootPath, "content");
string motdPath = Path.Combine(contentDir, "motd.txt");

string LoadMotd()
{
    try
    {
        if (File.Exists(motdPath)) {
            return File.ReadAllText(motdPath);
        }
    }
    catch { }
    return "Yikes, the server couldn't find a MOTD, and if it can't find a MOTD then you're probally BONED\n";
}

app.MapGet("/motd", (HttpContext ctx) =>
{
    ctx.Response.Headers.CacheControl = "no-cache";
    return Results.Text(LoadMotd(), "text/plain; charset=utf-8");
});

app.Run();