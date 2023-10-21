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
            new FileRoute("posts/{id}.html", "posts/[id].html"),
            new FileRoute("posts/{id}", "posts/[id].html"),
            new FileRoute("admin/page.html", "admin/page.html"),
            new FileRoute("admin/page", "admin/page.html"),
        };
        var fileProvider = new PhysicalFileProvider(
            Path.Combine(Environment.CurrentDirectory, "TestFiles")
        );

        var actual = NextJsRoutesProvider.GetFileRoutes(fileProvider).ToArray();
        Assert.Equal(expected, actual);
    }
}