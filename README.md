[![NuGet Gallery | Bymse.AspNetCore.NextJsStaticHosting](https://img.shields.io/nuget/v/Bymse.AspNetCore.NextJsStaticHosting)](https://www.nuget.org/packages/Bymse.AspNetCore.NextJsStaticHosting)
# Bymse.AspNetCore.NextJsStaticHosting

Library that enables you to host statically exported Next.js application within ASP.NET Core application.

## Features

- **Next.js pages hosting**. Serves static html pages exported from Next.js application. Includes support for dynamic routes.
- **Next.js static assets hosting**. Serves static assets exported from Next.js application.
- **Arbitrary sources of files**. Allows to serve files from any source, including embedded resources, physical files, etc.
- **Simple chunks version skew protection**. Allows to serve files from multiple versions of Next.js application.


## Usage

`next build && next export` can be used to build and [export](https://nextjs.org/docs/pages/building-your-application/deploying/static-exports) Next.js application.


1. Install `Bymse.AspNetCore.NextJsStaticHosting` NuGet package.
2. Add the following code to `Startup.cs` file:

```csharp
public void Configure(IApplicationBuilder app)
{
        app.UseRouting();

        var latestVersion = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "out", "latest")); 
        var previousDeployment = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "out", "previous")); // optional
        
        app.UseEndpoints(b => b.MapNextJsStaticEndpoints(new NextJsStaticEndpointsOptions(latestVersion)));

        app.UseNextJsStaticFiles(new NextJsStaticFilesOptions
        {
            FileProvider = new CompositeFileProvider(
                latestVersion,
                previousDeployment
            )
        });
}
```
3. Or for minimal API:
```csharp
using AspNetCore.NextJsStaticHosting;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var latestVersion = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "out", "latest")); 
var previousDeployment = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "out", "previous")); // optional

app.MapNextJsStaticEndpoints(new NextJsStaticEndpointsOptions(latestVersion));

app.UseNextJsStaticFiles(new NextJsStaticFilesOptions
{
    FileProvider = new CompositeFileProvider(
        latestVersion,
        previousDeployment
    )
});

app.Run();
```

Example use case can be found in `AspNetCore.NextJsStaticHosting.Tests` folder.

## Chunks version skew protection

Chunks version skew in a Single Page Application (SPA) refers to the situation where an application opened in a browser tries to fetch a chunk for the next page, but the chunk is not available on the server because the server was updated with a new version of the application. This situation is not handled by default by Next.js. This library provides a simple mechanism to handle this situation.

In `Bymse.AspNetCore.NextJsStaticHosting`` it is possible to provide multiple versions of Next.js static build as input for serving. It would follow the convention:

1. Html pages only from latest version of Next.js application are served. It allows to update in-browser version on refresh and prevents deleted pages from being available.
2. Static files are served from all available versions, but files from the latest version will have priority.

This way opened version can access old chunks and after refresh user will get new version of application.

## References

[NextjsStaticHosting-AspNetCore](https://github.com/davidnx/NextjsStaticHosting-AspNetCore) was used as a reference for endpoint map building. Thanks to the author https://github.com/davidnx.