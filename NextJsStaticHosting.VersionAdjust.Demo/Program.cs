var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/about", () => "about");
app.MapGet("/about/{**path}", () => "path");

app.Run();