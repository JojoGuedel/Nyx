namespace Nyx.Diagnostics;

public class DiagnosticWriter
{
    private TextWriter _writer;
    private TextInfo _textInfo;

    public DiagnosticWriter(TextWriter writer, string text)
    {
        _writer = writer;
        _textInfo = new TextInfo(text);
    }

    public void Write(DiagnosticCollection diagnostics)
    {
        foreach(var diagnostic in diagnostics.GetAll())
        {
            var line = _textInfo.GetLineAtIndex(diagnostic.location.pos);
            var relativePos = diagnostic.location.pos - line.start;
            
            Console.WriteLine($"{diagnostic.severity} durring {diagnostic.origin} ({diagnostic.kind} at Line {line.lineNumber}, {relativePos}):");
            Console.Write(line.line);

            for(int i = 0; i < relativePos-1; i++)
                Console.Write(" ");
            for (int i = 0; i < diagnostic.location.len; i++)
                Console.Write("~");
            Console.WriteLine();
            Console.WriteLine(diagnostic.GetMessage());
            Console.WriteLine();
        }
    }
}
