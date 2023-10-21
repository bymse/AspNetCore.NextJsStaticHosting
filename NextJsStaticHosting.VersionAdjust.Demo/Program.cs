using Microsoft.Extensions.FileProviders;
using NextJsStaticHosting.VersionAdjust;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var options = new NextJsStaticFilesOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out"),
};

app.UseNextJsStaticPages(options);
app.UseNextJsStaticFiles(options);

app.Run();