using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust;

public class NextJsStaticEndpointsOptions
{
    public IFileProvider FileProvider { get; init; }
    
    public string[] PathsToExclude { get; init; } = Array.Empty<string>();
}