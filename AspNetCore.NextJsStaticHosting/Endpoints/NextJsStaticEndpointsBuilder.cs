using AspNetCore.NextJsStaticHosting.Endpoints.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace AspNetCore.NextJsStaticHosting.Endpoints;

internal static class NextJsStaticEndpointsBuilder
{
    public static IEnumerable<Endpoint> Build(
        FileRoute[] routes,
        RequestDelegate requestDelegate
    )
    {
        foreach (var route in routes)
        {
            var builder = new RouteEndpointBuilder(
                requestDelegate: requestDelegate,
                routePattern: RoutePatternFactory.Parse(route.Route),
                order: 0
            );

            builder.Metadata.Add(new StaticEndpointMetadata(route.FilePath));
            builder.DisplayName = route.FilePath;

            yield return builder.Build();
        }
    }
}