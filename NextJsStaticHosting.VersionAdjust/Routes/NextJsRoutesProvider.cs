﻿using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust.Routes;

internal static class NextJsRoutesProvider
{
    public static IEnumerable<FileRoute> GetFileRoutes(IFileProvider fileProvider)
    {
        return GetFilesRelativePaths(fileProvider).SelectMany(GetRoute);
    }

    private static readonly Regex SlugRegex = new(@"\[(?<slug>[A-z0-9_\.]+)\]");
    private static readonly Regex HtmlEndingRegex = new(@"\.html$");

    private static IEnumerable<FileRoute> GetRoute(string path)
    {
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

    private static IEnumerable<string> GetFilesRelativePaths(IFileProvider fileProvider)
    {
        var dirs = new Stack<string>();
        dirs.Push("");

        while (dirs.Count > 0)
        {
            var dir = dirs.Pop();
            foreach (var content in fileProvider.GetDirectoryContents(dir))
            {
                var path = dir == "" ? content.Name : $"{dir}/{content.Name}";
                if (content.IsDirectory)
                {
                    dirs.Push(path);
                }
                else
                {
                    yield return path;
                }
            }
        }
    }
}