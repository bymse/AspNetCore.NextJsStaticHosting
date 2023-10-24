namespace AspNetCore.NextJsStaticHosting.Endpoints;

internal class StaticEndpointMetadata
{
    public StaticEndpointMetadata(string path)
    {
        Path = path;
    }

    public string Path { get; }
}