using Microsoft.AspNetCore.Builder;

namespace AspNetCore.NextJsStaticHosting;

public class NextJsStaticFilesOptions : StaticFileOptions
{
    public NextJsStaticFilesOptions()
    {
        OnPrepareResponse = context =>
        {
            context.Context.Response.StatusCode = context.File.Name == Constants.NOT_FOUND_PAGE
                ? 404
                : context.Context.Response.StatusCode;
        };
    }
}