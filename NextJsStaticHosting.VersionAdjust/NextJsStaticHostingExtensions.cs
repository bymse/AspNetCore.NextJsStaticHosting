using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextJsStaticHosting.VersionAdjust.Endpoints;

namespace NextJsStaticHosting.VersionAdjust;

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
            throw new ArgumentNullException(nameof(options.FileProvider));
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
        applicationBuilder.UseMiddleware<PageNotFoundMiddleware>();
        
        return applicationBuilder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = options.FileProvider,
            OnPrepareResponse = context =>
            {
                context.Context.Response.StatusCode = context.File.Name == Constants.NOT_FOUND_PAGE
                    ?  404
                    : context.Context.Response.StatusCode;
            } 
        });
    }
}