using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using NextJsStaticHosting.VersionAdjust.Endpoints.Routes;

namespace NextJsStaticHosting.VersionAdjust.Endpoints;

internal static class NextJsPageEndpointsBuilder
{
    public static IEnumerable<Endpoint> Build(
        FileRoute[] routes,
        IEndpointRouteBuilder endpoints,
        NextJsStaticFilesOptions options
    )
    {
        var requestDelegate = GetRequestDelegate(endpoints, options);
        foreach (var route in routes)
        {
            var builder = new RouteEndpointBuilder(
                requestDelegate: requestDelegate,
                routePattern: RoutePatternFactory.Parse(route.Route),
                order: 0
            );

            builder.Metadata.Add(new HtmlEndpointMetadata(route.FilePath));
            builder.DisplayName = route.FilePath;

            yield return builder.Build();
        }
    }

    private static RequestDelegate GetRequestDelegate(IEndpointRouteBuilder endpoints, NextJsStaticFilesOptions options)
    {
        var app = endpoints.CreateApplicationBuilder();
        app.Use(next => context =>
        {
            var endpoint = context.GetEndpoint();
            var metadata = endpoint?.Metadata.GetMetadata<HtmlEndpointMetadata>();
            if (metadata == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(HtmlEndpointMetadata)} not found on endpoint: {endpoint}"
                );
            }

            context.Request.Path = "/" + metadata.Path;
            context.SetEndpoint(null);

            return next(context);
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = options.OnPrepareResponse ?? (_ => { }),
            FileProvider = options.FileProvider
        });

        return app.Build();
    }

    private class HtmlEndpointMetadata
    {
        public HtmlEndpointMetadata(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}