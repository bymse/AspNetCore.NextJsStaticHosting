using AspNetCore.NextJsStaticHosting.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.NextJsStaticHosting;

public static class NextJsStaticHostingExtensions
{
    public static IEndpointRouteBuilder MapNextJsStaticEndpoints(this IEndpointRouteBuilder endpoints,
        NextJsStaticEndpointsOptions options)
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
            throw new NullReferenceException(nameof(options) + "." + nameof(options.FileProvider));
        }

        var dataSource = new NextJsStaticEndpointsDataSource(endpoints, options);
        endpoints.DataSources.Add(dataSource);

        return endpoints;
    }

    public static IApplicationBuilder UseNextJsStaticFiles(
        this IApplicationBuilder applicationBuilder,
        NextJsStaticFilesOptions options
    )
    {
        applicationBuilder.UseMiddleware<StaticEndpointNotFoundMiddleware>();
        return applicationBuilder.UseStaticFiles(options);
    }
}