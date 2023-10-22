using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using NextJsStaticHosting.VersionAdjust.Endpoints.Routes;

namespace NextJsStaticHosting.VersionAdjust.Endpoints;

internal class NextJsStaticEndpointsDataSource : EndpointDataSource
{
    private readonly Lazy<IReadOnlyList<Endpoint>> endpoints;

    public NextJsStaticEndpointsDataSource(IEndpointRouteBuilder endpointsBuilder, NextJsStaticEndpointsOptions options)
    {
        endpoints = new Lazy<IReadOnlyList<Endpoint>>(() =>
        {
            var routes = NextJsStaticRoutesProvider
                .GetRoutes(options.FileProvider, options.PathsToExclude)
                .ToArray();

            return NextJsStaticEndpointsBuilder
                .Build(routes, endpointsBuilder, options)
                .ToArray();
        });
    }

    public override IReadOnlyList<Endpoint> Endpoints => endpoints.Value;

    public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;
}