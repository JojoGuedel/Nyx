namespace Nyx.Diagnostics;

public class DiagnosticCollection
{
    private List<Diagnostic> _diagnostics;

    public bool containsAny { get => _diagnostics.Count > 0; }
    public bool containsErrors { get => _diagnostics.Any((a) => a.severity == DiagnosticSeverity.Error); }
    public bool containsWarnings { get => _diagnostics.Any((a) => a.severity == DiagnosticSeverity.Warning); }

    public DiagnosticCollection()
    {
        _diagnostics = new List<Diagnostic>();
    }

    public void Add(Diagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
    }

    public IEnumerable<Diagnostic> GetAll()
    {
        // TODO: make this better
        foreach(var diagnostic in _diagnostics)
            yield return diagnostic;
    }
}

public class TextInfo
{
    public int length { get; }
    public List<LineInfo> lines { get; }

    public TextInfo(string text)
    {
        length = text.Length;
        lines = new List<LineInfo>();

        var line = string.Empty;
        var start = 0;

        for(int i = 0; i < text.Length; i++)
        {
            line += text[i];

            if (text[i] == '\n')
            {
                lines.Add(new LineInfo(line, i + 1, start));
                line = string.Empty;
                start = i;
            }
        }
    }

    public LineInfo GetLineAtIndex(int pos)
    {
        var last = 0;

        for(int i = 0; i < lines.Count - 1; i++)
        {
            if (lines[i].start < pos)
                last = i;
            else
                break;
        }

        return lines[last];
    }
}

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
}
