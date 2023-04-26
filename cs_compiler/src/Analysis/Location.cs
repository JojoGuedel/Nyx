using System.Diagnostics;

namespace Nyx.Analysis;

internal class Location
{
    internal int begin { get; }
    internal int end { get; }
    internal int lineBegin { get; }
    internal int lineEnd { get; }

    internal int length { get => end - begin; }

    internal Location(int index, int end, int lineBegin, int lineEnd)
    {
        Debug.Assert(end >= begin);
        Debug.Assert(lineEnd >= lineBegin);

        this.begin = index;
        this.end = end;
        this.lineBegin = lineBegin;
        this.lineEnd = lineEnd;
    }

    internal Location Point() => new Location(end, end, lineEnd, lineEnd);

    internal static Location Empty() => new Location(0, 0, 1, 1);

    internal static Location Embrace(Location a, Location b) => new Location(a.begin, b.end, a.lineBegin, b.lineEnd);
}