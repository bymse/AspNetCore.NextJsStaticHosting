using Microsoft.Extensions.FileProviders;
using AspNetCore.NextJsStaticHosting.Endpoints.Routes;
using Xunit;

namespace AspNetCore.NextJsStaticHosting.Tests;

public class NextJsRoutesProviderTests
{
    [Fact]
    public void ShouldReturnCorrectStructure()
    {
        var expected = new[]
            {
                new FileRoute("", "index.html"),
                new FileRoute("index.html", "index.html"),
                new FileRoute("about", "about.html"),
                new FileRoute("about.html", "about.html"),
                new FileRoute("404", "404.html"),
                new FileRoute("404.html", "404.html"),
                new FileRoute("id/{id}", "id/[id].html"),
                new FileRoute("id/{id}.html", "id/[id].html"),
                new FileRoute("two-slugs/{first}/{second}", "two-slugs/[first]/[second].html"),
                new FileRoute("two-slugs/{first}/{second}.html", "two-slugs/[first]/[second].html"),
                new FileRoute("inner/page", "inner/page.html"),
                new FileRoute("inner/page.html", "inner/page.html"),
                new FileRoute("catchall/{**slug}", "catchall/[...slug].html"),
                new FileRoute("catchalloptional/{**slug}", "catchalloptional/[[...slug]].html"),
            }.OrderBy(e => e.Route)
            .ToArray();

        var fileProvider = new PhysicalFileProvider(
            TestFilesPathProvider.CurrentVersion
        );

        var actual = NextJsStaticRoutesProvider
            .GetRoutes(fileProvider, Array.Empty<string>())
            .OrderBy(e => e.Route)
            .ToArray();

        Assert.Equal(expected, actual);
    }
}