using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust;

public class NextJsStaticEndpointsOptions
{
    public NextJsStaticEndpointsOptions(IFileProvider fileProvider)
    {
        FileProvider = fileProvider;
    }

    public IFileProvider FileProvider { get; }

    public string[] PathsToExclude { get; init; } = { "_next" };
}