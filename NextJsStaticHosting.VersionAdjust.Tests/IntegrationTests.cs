using System.Net;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace NextJsStaticHosting.VersionAdjust.Tests;

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
    public async Task ShouldReturnCurrentVersion_OfStaticRoutes(string path, string expectedContent)
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
    public async Task ShouldReturnCurrentVersion_OfDynamicRoutes(string path, string expectedContent)
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
    public async Task ShouldReturnCurrentVersion_OfStaticFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }
    
    [Theory]
    [InlineData("/image.png", "current-image")]
    public async Task ShouldReturnCurrentVersion_OfNonHtmlFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }
    
    [Theory]
    [InlineData("/old-image.png", "prev-image")]
    public async Task ShouldReturnPreviousVersion_OfRemovedFromCurrent_NonHtmlFiles(string path, string expectedContent)
    {
        await AssertPathContent(path, expectedContent);
    }

    [Theory]
    [InlineData("/old")]
    [InlineData("/old.html")]
    [InlineData("/deprecated/page")]
    [InlineData("/deprecated/page.html")]
    [InlineData("/deprecated/123")]
    public async Task ShouldReturn404_OfPreviousVersionRoutes(string path)
    {
        var response = await httpClient.GetAsync(path);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("404", content);
    }

    private async Task AssertPathContent(string path, string expectedContent)
    {
        var response = await httpClient.GetStringAsync(path);
        Assert.Equal(expectedContent, response);
    }

    public void Dispose()
    {
        testServer.Dispose();
    }
}