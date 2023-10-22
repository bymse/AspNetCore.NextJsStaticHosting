using Microsoft.Extensions.FileProviders;

namespace AspNetCore.NextJsStaticHosting;

public class NextJsStaticEndpointsOptions
{
    public NextJsStaticEndpointsOptions(IFileProvider fileProvider)
    {
        FileProvider = fileProvider;
    }

    public IFileProvider FileProvider { get; }

    public string[] PathsToExclude { get; init; } = { "_next" };
}