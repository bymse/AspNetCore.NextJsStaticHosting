﻿using AspNetCore.NextJsStaticHosting.Endpoints.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace AspNetCore.NextJsStaticHosting.Endpoints;

internal class NextJsStaticEndpointsDataSource : EndpointDataSource
{
    private readonly object @lock = new();
    private Endpoint[] endpoints = Array.Empty<Endpoint>();
    private CancellationTokenSource? cancellationTokenSource;
    private IChangeToken changeToken = NullChangeToken.Singleton;

    private readonly RequestDelegate requestDelegate;
    private readonly NextJsStaticEndpointsOptions options;

    public NextJsStaticEndpointsDataSource(IEndpointRouteBuilder endpointsBuilder, NextJsStaticEndpointsOptions options)
    {
        this.options = options;
        requestDelegate = NextJsStaticEndpointDelegateBuilder.Build(endpointsBuilder, options);
        LoadEndpoints();

        if (options.EnableEndpointRebuildOnChange)
        {
            ReinitChangeToken();
            InitializeWatch();
        }
    }

    private void OnFileChange(object _)
    {
        lock (@lock)
        {
            InitializeWatch();
            LoadEndpoints();
            ReinitChangeToken();
        }
    }

    private void InitializeWatch()
    {
        options.FileProvider
            .Watch("**")
            .RegisterChangeCallback(OnFileChange, null);
    }

    private void LoadEndpoints()
    {
        var routes = NextJsStaticRoutesProvider
            .GetRoutes(options.FileProvider, options.PathsToExclude)
            .ToArray();

        var arr = NextJsStaticEndpointsBuilder
            .Build(routes, requestDelegate)
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