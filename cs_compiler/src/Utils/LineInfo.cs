namespace Nyx.Utils;

public class LineInfo
{
    public string line { get; }
    public int lineNumber { get; }
    public int start { get; }

    public LineInfo(string line, int lineNumber, int start)
    {
        this.line = line;
        this.lineNumber = lineNumber;
        this.start = start;
    }

    public override string ToString()
    {
        var ret = $"{lineNumber}|{line}";

        if (line.Length == 0 || line.Last() != '\n')
            ret += '\n';

        return ret;
    }
}
