using Microsoft.Extensions.FileProviders;
using NextJsStaticHosting.VersionAdjust.Routes;
using NSubstitute;
using Xunit;

namespace NextJsStaticHosting.VersionAdjust.Tests;

public class NextJsRawRoutesProviderTest
{
    [Fact]
    public void ShouldReturnRoot()
    {
        var expected = new[]
        {
            new FileRoute("index.html", "index.html"),
            new FileRoute("/", "index.html"),
        };
        var mock = MockFileSystem(new Dictionary<string, string[]>
        {
            {
                "", new[]
                {
                    "index.html"
                }
            }
        });

        var actual = NextJsRoutesProvider.GetFileRoutes(mock).ToArray();
        Assert.Equal(expected, actual);
    }

    private static IFileProvider MockFileSystem(IDictionary<string, string[]> fileSystem)
    {
        var mock = Substitute.For<IFileProvider>();

        foreach (var (dir, contents) in fileSystem)
        {
            var mocks = contents.Select(e =>
            {
                var fileInfo = Substitute.For<IFileInfo>();
                fileInfo.Name.Returns(e);
                fileInfo.IsDirectory.Returns(!e.EndsWith(".html"));
                return fileInfo;
            });

            mock
                .GetDirectoryContents(dir)
                .Returns(new FakeDirectoryContents(mocks));
        }

        return mock;
    }
}