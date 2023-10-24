using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace AspNetCore.NextJsStaticHosting.Tests;

public class IntegrationTests : IDisposable
{
    private readonly TestServer testServer;

    private readonly HttpClient httpClient;

    public IntegrationTests()
    {
        var webHostBuilder = new WebHostBuilder()
            .UseStartup<TestStartup>();

        testServer = new TestServer(webHostBuilder);
        httpClient = testServer.CreateClient();
    }

    [Theory]
    [InlineData("/", "index")]
    [InlineData("/index.html", "index")]
    [InlineData("/about.html", "about")]
    [InlineData("/about", "about")]
    [InlineData("/inner/page.html", "inner-page")]
    [InlineData("/inner/page", "inner-page")]
    public async Task ShouldReturnCurrentVersion_OnStaticRoutes(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/two-slugs/1/2", "[first][second]")]
    [InlineData("/two-slugs/1093/2398", "[first][second]")]
    [InlineData("/id/2398", "[id]")]
    [InlineData("/id/test", "[id]")]
    [InlineData("/catchall/other", "catch all")]
    [InlineData("/catchall/first/second", "catch all")]
    [InlineData("/catchalloptional/other", "catch all optional")]
    [InlineData("/catchalloptional/first/second", "catch all optional")]
    public async Task ShouldReturnCurrentVersion_OnDynamicRoutes(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/_next/static/chunks/main-current.js", "main-current")]
    [InlineData("/_next/static/chunks/pages/index-current.js", "index-current")]
    [InlineData("/_next/static/chunks/pages/id/[id]-current.js", "[id]-current")]
    [InlineData("/_next/static/css/first-current.css", "first-current")]
    [InlineData("/_next/static/css/second-current.css", "second-current")]
    [InlineData("/_next/static/hash-current/_buildManifest.js", "_buildManifest")]
    [InlineData("/_next/static/hash-current/_ssgManifest.js", "_ssgManifest")]
    [InlineData("/_next/static/media/font-current.woff2", "font-current")]
    public async Task ShouldReturnCurrentVersion_OnStaticFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/image.png", "current-image")]
    public async Task ShouldReturnCurrentVersion_OnNonHtmlFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/old-image.png", "prev-image")]
    public async Task ShouldReturnPreviousVersion_OnRemovedFromCurrent_NonHtmlFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/_next/static/chunks/main-prev.js", "main-prev")]
    [InlineData("/_next/static/chunks/pages/index-prev.js", "index-prev")]
    [InlineData("/_next/static/chunks/pages/id/[id]-prev.js", "[id]-prev")]
    [InlineData("/_next/static/css/first-prev.css", "first-prev")]
    [InlineData("/_next/static/css/second-prev.css", "second-prev")]
    [InlineData("/_next/static/hash-prev/_buildManifest.js", "prev-_buildManifest")]
    [InlineData("/_next/static/hash-prev/_ssgManifest.js", "prev-_ssgManifest")]
    [InlineData("/_next/static/media/font-prev.woff2", "prev-font")]
    public async Task ShouldReturnPreviousVersion_OnStaticFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/old")]
    [InlineData("/old.html")]
    [InlineData("/deprecated/page")]
    [InlineData("/deprecated/page.html")]
    [InlineData("/deprecated/123")]
    public async Task ShouldReturn404_OnPreviousVersionRoutes(string path)
    {
        await AssertNotFound(path, "404");
    }

    [Theory]
    [InlineData("/random.png")]
    [InlineData("/_next/static/chunks/pages/random.js")]
    public async Task ShouldReturnEmpty404_OnNonExistentPath(string path)
    {
        await AssertNotFound(path, "");
    }

    [Theory]
    [InlineData("/{0}.html", "/{0}.html")]
    [InlineData("/{0}.html", "/{0}")]
    [InlineData("/random/{0}.html", "/random/{0}.html")]
    [InlineData("/random/{0}.html", "/random/{0}")]
    [InlineData("/random2/[id].html", "/random2/{0}.html")]
    [InlineData("/random2/[id].html", "/random2/{0}")]
    [InlineData("/_next/static/chunks/{0}.js", "/_next/static/chunks/{0}.js", "")]
    public async Task ShouldReturn_NewlyCreatedFile(
        string filePathTemplate,
        string routeTemplate,
        string notFoundContent = "404"
    )
    {
        var key = Guid.NewGuid().ToString();
        var filePath = string.Format(filePathTemplate, key);
        var route = string.Format(routeTemplate, key);

        await AssertNotFound(route, notFoundContent);

        var pathToFile = Path.Combine(TestFilesPathProvider.CurrentVersion, filePath.TrimStart('/'));
        FileHelpers.WriteFileRecursively(pathToFile, key);

        Thread.Sleep(1000);

        await AssertPathContent(route, key);

        File.Delete(pathToFile);
    }

    private async Task AssertPathContent(string path, string expectedContent)
    {
        var response = await httpClient.GetStringAsync(path);
        Assert.Equal(expectedContent, response);
    }

    private async Task AssertNotFound(string path, string expectedContent)
    {
        var response = await httpClient.GetAsync(path);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(expectedContent, content);
    }

    public void Dispose()
    {
        testServer.Dispose();
    }
}