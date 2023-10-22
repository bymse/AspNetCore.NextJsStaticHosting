using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust;

public class NextJsStaticFilesOptions
{
    public IFileProvider FileProvider { get; init; }
}