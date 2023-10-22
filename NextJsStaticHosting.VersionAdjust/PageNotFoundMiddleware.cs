using Microsoft.AspNetCore.Http;

namespace NextJsStaticHosting.VersionAdjust;

public class PageNotFoundMiddleware
{
    private readonly RequestDelegate next;

    public PageNotFoundMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            return next(context);
        }

        var isPageRequest = !context.Request.Path.HasValue ||
                            context.Request.Path.Value.EndsWith(".html") ||
                            !context.Request.Path.Value.Contains('.');
        if (isPageRequest)
        {
            context.Request.Path = "/" + Constants.NOT_FOUND_PAGE;
        }

        return next(context);
    }
}