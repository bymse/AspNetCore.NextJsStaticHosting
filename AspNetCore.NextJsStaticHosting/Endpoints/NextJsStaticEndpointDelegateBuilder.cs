using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.NextJsStaticHosting.Endpoints;

internal static class NextJsStaticEndpointDelegateBuilder
{
    public static RequestDelegate Build(IEndpointRouteBuilder endpoints, NextJsStaticEndpointsOptions options)
    {
        var app = endpoints.CreateApplicationBuilder();
        app.Use(next => context =>
        {
            var endpoint = context.GetEndpoint();
            var metadata = endpoint?.Metadata.GetMetadata<StaticEndpointMetadata>();
            if (metadata == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(StaticEndpointMetadata)} not found on endpoint: {endpoint}"
                );
            }

            context.Request.Path = "/" + metadata.Path;
            context.SetEndpoint(null);

            return next(context);
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = options.FileProvider
        });

        return app.Build();
    }
}