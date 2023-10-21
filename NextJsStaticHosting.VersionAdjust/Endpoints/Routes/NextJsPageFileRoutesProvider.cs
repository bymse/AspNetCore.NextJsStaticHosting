using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust.Endpoints.Routes;

internal static class NextJsPageFileRoutesProvider
{
    public static IEnumerable<FileRoute> GetRoutes(IFileProvider fileProvider, params string[] dirsToExclude)
    {
        return GetFilesRelativePaths(fileProvider, dirsToExclude).SelectMany(GetRoute);
    }

    private static readonly Regex SlugRegex = new(@"\[(?<slug>(?!\[?\.\.\.)[A-z0-9_]+?)\]", RegexOptions.Compiled);
    private static readonly Regex HtmlEndingRegex = new(@"\.html$", RegexOptions.Compiled);

    private static readonly Regex CatchAllRegex =
        new(@"\[{1,2}\.{3}(?<slug>[A-z0-9_]+?)\]{1,2}.html", RegexOptions.Compiled);

    private static IEnumerable<FileRoute> GetRoute(string path)
    {
        if (CatchAllRegex.IsMatch(path))
        {
            var replaced = CatchAllRegex.Replace(path, match => $"{{**{match.Groups["slug"].Value}}}");
            yield return new FileRoute(replaced, path);
            yield break;
        }

        var route = SlugRegex.Replace(path, match => $"{{{match.Groups["slug"].Value}}}");
        yield return new FileRoute(route, path);

        if (path.Equals("index.html"))
        {
            yield return new FileRoute("", path);
        }
        else if (path.EndsWith(".html"))
        {
            yield return new FileRoute(HtmlEndingRegex.Replace(route, ""), path);
        }
    }

    private static IEnumerable<string> GetFilesRelativePaths(IFileProvider fileProvider, string[] dirsToExclude)
    {
        var dirs = new Stack<string>();
        dirs.Push("");

        while (dirs.Count > 0)
        {
            var dir = dirs.Pop();
            foreach (var content in fileProvider.GetDirectoryContents(dir))
            {
                var path = dir == "" ? content.Name : $"{dir}/{content.Name}";
                if (content.IsDirectory && !dirsToExclude.Contains(path))
                {
                    dirs.Push(path);
                }
                else if (Path.GetExtension(content.Name) == ".html")
                {
                    yield return path;
                }
            }
        }
    }
}