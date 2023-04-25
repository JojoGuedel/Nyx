using System.Diagnostics;

namespace Nyx.Analysis;

internal class Location
{
    internal string fileName { get; }
    internal long index { get; }
    internal long length { get; }
    internal long end { get => index + end; }
    internal long lineBegin { get; }
    internal long lineEnd { get; }

    internal Location(string fileName, long index, long length, long lineBegin, long lineEnd)
    {
        this.fileName = fileName;
        this.index = index;
        this.length = length;
        this.lineBegin = lineBegin;
        this.lineEnd = lineEnd;
    }

    internal Location Point() => new Location(fileName, end, 0, lineEnd, lineEnd);

    internal static Location Embrace(Location a, Location b)
    {
        Debug.Assert(a.fileName == b.fileName);

        return new Location(a.fileName, a.index, b.end - a.index, a.lineBegin, b.lineEnd);
    }
}