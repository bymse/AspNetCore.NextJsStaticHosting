using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using NextJsStaticHosting.VersionAdjust.Endpoints.Routes;

namespace NextJsStaticHosting.VersionAdjust.Endpoints;

public class NextJsPagesEndpointsDataSource : EndpointDataSource
{
    private readonly Lazy<IReadOnlyList<Endpoint>> endpoints;

    public NextJsPagesEndpointsDataSource(IEndpointRouteBuilder endpointsBuilder, NextJsStaticFilesOptions options)
    {
        endpoints = new Lazy<IReadOnlyList<Endpoint>>(() =>
        {
            var routes = NextJsPageFileRoutesProvider
                .GetRoutes(options.FileProvider!, options.StaticBuildDir)
                .ToArray();

            return NextJsPageEndpointsBuilder
                .Build(routes, endpointsBuilder, options)
                .ToArray();
        });
    }

    public override IReadOnlyList<Endpoint> Endpoints => endpoints.Value;

    public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;
}