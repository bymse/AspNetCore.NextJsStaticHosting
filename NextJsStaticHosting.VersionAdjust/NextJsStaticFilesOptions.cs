using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust;

public class NextJsStaticFilesOptions
{
    public IFileProvider FileProvider { get; init; }
    
    public Action<StaticFileResponseContext> OnPrepareResponse { get; set; }
}