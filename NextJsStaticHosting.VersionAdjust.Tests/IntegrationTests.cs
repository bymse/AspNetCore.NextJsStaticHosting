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
    public async Task ShouldReturnCurrentVersionOfPages(string path, string expectedContent)
    {
        var response = await httpClient.GetStringAsync(path);
        Assert.Equal(expectedContent, response);
    }

    public void Dispose()
    {
        testServer.Dispose();
    }
}