using Microsoft.Extensions.FileProviders;
using NextJsStaticHosting.VersionAdjust;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapNextJsStaticEndpoints(
    new NextJsStaticEndpointsOptions(
        new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out")
    )
);

app.UseNextJsStaticFiles(new NextJsStaticFilesOptions
{
    FileProvider = new CompositeFileProvider(
        new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out"),
        new PhysicalFileProvider(builder.Environment.ContentRootPath + "/client-app/out2")
    )
});

app.Run();