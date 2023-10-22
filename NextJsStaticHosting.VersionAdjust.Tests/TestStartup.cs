﻿using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust.Tests;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseMiddleware<PageNotFoundMiddleware>();

        app.UseEndpoints(b => b.UseNextJsStaticPages(new NextJsStaticPagesOptions
        {
            FileProvider = new PhysicalFileProvider(TestFilesPathProvider.CurrentVersion),
            PathsToExclude = new[]
            {
                "_next"
            }
        }));

        app.UseNextJsStaticFiles(new NextJsStaticFilesOptions
        {
            FileProvider = new CompositeFileProvider(
                new PhysicalFileProvider(TestFilesPathProvider.CurrentVersion),
                new PhysicalFileProvider(TestFilesPathProvider.PreviousVersion)
            )
        });
    }
}