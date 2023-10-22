using Microsoft.Extensions.FileProviders;
using NextJsStaticHosting.VersionAdjust;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseNextJsStaticPages(new NextJsStaticPagesOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out"),
    PathsToExclude = new[]
    {
        "_next"
    }
});

app.UseMiddleware<PageNotFoundMiddleware>();

app.UseNextJsStaticFiles(new NextJsStaticFilesOptions
{
    FileProvider = new CompositeFileProvider(
        new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out"),
        new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out2")
        )
});

app.Run();