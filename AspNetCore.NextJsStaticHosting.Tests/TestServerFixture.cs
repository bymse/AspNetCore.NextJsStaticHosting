using Microsoft.AspNetCore.TestHost;

namespace AspNetCore.NextJsStaticHosting.Tests;

public class TestServerFixture : IDisposable
{
    public TestServerFixture()
    {
        var webHostBuilder = new WebHostBuilder()
            .UseStartup<TestStartup>();

        TestServer = new TestServer(webHostBuilder);
        HttpClient = TestServer.CreateClient();
    }

    public TestServer TestServer { get; }

    public HttpClient HttpClient { get; }

    public void Dispose()
    {
        TestServer.Dispose();
    }
}