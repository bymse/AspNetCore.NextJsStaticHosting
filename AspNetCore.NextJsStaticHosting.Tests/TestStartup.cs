﻿using Microsoft.Extensions.FileProviders;

namespace AspNetCore.NextJsStaticHosting.Tests;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(b => b.MapNextJsStaticEndpoints(new NextJsStaticEndpointsOptions(
            new PhysicalFileProvider(TestFilesPathProvider.CurrentVersion)
        )));

        app.UseNextJsStaticFiles(new NextJsStaticFilesOptions
        {
            FileProvider = new CompositeFileProvider(
                new PhysicalFileProvider(TestFilesPathProvider.CurrentVersion),
                new PhysicalFileProvider(TestFilesPathProvider.PreviousVersion)
            )
        });
    }
}