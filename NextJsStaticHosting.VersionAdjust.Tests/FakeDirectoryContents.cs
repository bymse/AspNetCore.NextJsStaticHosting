using System.Collections;
using Microsoft.Extensions.FileProviders;

namespace NextJsStaticHosting.VersionAdjust.Tests;

public class FakeDirectoryContents : IDirectoryContents
{
    private readonly IEnumerable<IFileInfo> files;

    public FakeDirectoryContents(IEnumerable<IFileInfo> files)
    {
        this.files = files;
    }
    
    public IEnumerator<IFileInfo> GetEnumerator()
    {
        return files.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Exists => true;
}