using AspNetCore.NextJsStaticHosting.Endpoints.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace AspNetCore.NextJsStaticHosting.Endpoints;

internal class NextJsStaticEndpointsDataSource : EndpointDataSource
{
    private readonly object @lock = new(); 
    private Endpoint[] endpoints;
    private CancellationTokenSource? cancellationTokenSource;
    private IChangeToken changeToken;

    private readonly IEndpointRouteBuilder endpointsBuilder;
    private readonly NextJsStaticEndpointsOptions options;

    public NextJsStaticEndpointsDataSource(IEndpointRouteBuilder endpointsBuilder, NextJsStaticEndpointsOptions options)
    {
        this.endpointsBuilder = endpointsBuilder;
        this.options = options;
        LoadEndpoints();

        if (options.EnableEndpointRebuildOnChange)
        {
            ReinitChangeToken();
            options.FileProvider
                .Watch("**/*.html")
                .RegisterChangeCallback(OnFileChange, null);
        }
        else
        {
            changeToken = NullChangeToken.Singleton;
        }
    }

    private void OnFileChange(object _)
    {
        lock (@lock)
        {
            LoadEndpoints();
            ReinitChangeToken();
        }
    }

    private void LoadEndpoints()
    {
        var routes = NextJsStaticRoutesProvider
            .GetRoutes(options.FileProvider, options.PathsToExclude)
            .ToArray();

        var arr = NextJsStaticEndpointsBuilder
            .Build(routes, endpointsBuilder, options)
            .ToArray();

        endpoints = arr;
    }

    private void ReinitChangeToken()
    {
        var oldCancellationTokenSource = cancellationTokenSource;
        cancellationTokenSource = new CancellationTokenSource();
        changeToken = new CancellationChangeToken(cancellationTokenSource.Token);

        oldCancellationTokenSource?.Cancel();
    }

    public override IReadOnlyList<Endpoint> Endpoints => endpoints;

    public override IChangeToken GetChangeToken() => changeToken;
}