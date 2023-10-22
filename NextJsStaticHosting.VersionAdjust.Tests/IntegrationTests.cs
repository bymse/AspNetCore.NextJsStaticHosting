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
        var response = await httpClient.GetStringAsync(path);
        Assert.Equal(expectedContent, response);
    }
    
    [Theory]
    [InlineData("/1/2", "[first][second]")]
    [InlineData("/1093/2398", "[first][second]")]
    [InlineData("/id/2398", "[id]")]
    [InlineData("/id/test", "[id]")]
    [InlineData("/catchall/other", "catch all")]
    [InlineData("/catchall/first/second", "catch all")]
    [InlineData("/catchalloptional/other", "catch all optional")]
    [InlineData("/catchalloptional/first/second", "catch all optional")]
    public async Task ShouldReturnCurrentVersion_OfDynamicRoutes(string path, string expectedContent)
    {
        var response = await httpClient.GetStringAsync(path);
        Assert.Equal(expectedContent, response);
    }

    public void Dispose()
    {
        testServer.Dispose();
    }
}