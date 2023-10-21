using Microsoft.Extensions.FileProviders;
using NextJsStaticHosting.VersionAdjust.Routes;
using NSubstitute;
using Xunit;

namespace NextJsStaticHosting.VersionAdjust.Tests;

public class NextJsRoutesProviderTests
{
    [Fact]
    public void ShouldReturnCorrectStructure()
    {
        var expected = new[]
            {
                new FileRoute("about.html", "about.html"),
                new FileRoute("about", "about.html"),
                new FileRoute("index.html", "index.html"),
                new FileRoute("", "index.html"),
                new FileRoute("idslug/{id}.html", "idslug/[id].html"),
                new FileRoute("idslug/{id}", "idslug/[id].html"),
                new FileRoute("inner/page.html", "inner/page.html"),
                new FileRoute("inner/page", "inner/page.html"),
                new FileRoute("catchall/{**slug}", "catchall/[...slug].html"),
                new FileRoute("catchalloptional/{**slug}", "catchalloptional/[[...slug]].html"),
                
            }.OrderBy(e => e.Route)
            .ToArray();
        var fileProvider = new PhysicalFileProvider(
            Path.Combine(Environment.CurrentDirectory, "TestFiles")
        );

        var actual = NextJsRoutesProvider
            .GetFileRoutes(fileProvider)
            .OrderBy(e => e.Route)
            .ToArray();

        Assert.Equal(expected, actual);
    }
}