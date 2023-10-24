namespace AspNetCore.NextJsStaticHosting.Tests;

public static class FileHelpers
{
    public static void WriteFileRecursively(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, content);
    }
}