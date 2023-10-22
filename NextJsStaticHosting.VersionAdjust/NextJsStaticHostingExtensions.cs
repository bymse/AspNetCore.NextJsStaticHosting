using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextJsStaticHosting.VersionAdjust.Endpoints;

namespace NextJsStaticHosting.VersionAdjust;

public static class NextJsStaticHostingExtensions
{
    public static IEndpointRouteBuilder UseNextJsStaticPages(this IEndpointRouteBuilder endpoints,
        NextJsStaticPagesOptions options)
    {
        if (endpoints is null)
        {
            throw new ArgumentNullException(nameof(endpoints));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (options.FileProvider == null)
        {
            throw new ArgumentNullException(nameof(options.FileProvider));
        }

        var dataSource = new NextJsPagesEndpointsDataSource(endpoints, options);
        endpoints.DataSources.Add(dataSource);

        return endpoints;
    }

    public static IApplicationBuilder UseNextJsStaticFiles(
        this IApplicationBuilder applicationBuilder,
        NextJsStaticFilesOptions options
    )
    {
        return applicationBuilder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = options.FileProvider,
        });
    }
}