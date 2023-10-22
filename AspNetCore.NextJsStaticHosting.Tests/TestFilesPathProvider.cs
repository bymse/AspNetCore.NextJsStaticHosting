namespace AspNetCore.NextJsStaticHosting.Tests;

public static class TestFilesPathProvider
{
    public static string CurrentVersion => GetTestFilesPath(nameof(CurrentVersion));
    public static string PreviousVersion => GetTestFilesPath(nameof(PreviousVersion));
    
    private static string GetTestFilesPath(string version)
    {
        var testAssembly = typeof(TestFilesPathProvider).Assembly;
        var testAssemblyPath = testAssembly.Location;
        var testFilesPath = Path.Combine(Path.GetDirectoryName(testAssemblyPath)!, "TestFiles");
        return Path.Combine(testFilesPath, version);
    }
}