using Nyx.Utils;

namespace Nyx.Diagnostics;

public class DiagnosticWriter
{
    private TextWriter _writer;
    private TextInfo _textInfo;
    private int _lineSpy;

    public DiagnosticWriter(TextWriter writer, string text, int lineSpy = 1)
    {
        _writer = writer;
        _textInfo = new TextInfo(text);
        _lineSpy = lineSpy;
    }

    public void Write(DiagnosticCollection diagnostics)
    {
        foreach(var diagnostic in diagnostics.GetAll())
        {
            var line = _textInfo.GetLineAtIndex(diagnostic.location.pos);
            var lineNumber = line.lineNumber.ToString();
            var relativePos = diagnostic.location.pos - line.start;
            
            Console.WriteLine($"{diagnostic.severity} during {diagnostic.origin} ({diagnostic.kind} on Line {line.lineNumber}, {relativePos}):");

            for (int i = 0; i < _lineSpy; i++)
            {
                var preLine = line.lineNumber - _lineSpy + i;
                if (preLine > 0)
                {
                    for(int j = 0; j < lineNumber.Length - preLine.ToString().Length; j++)
                        Console.Write(" ");

                    Console.Write(_textInfo[line.lineNumber - (_lineSpy + 1) + i]);
                }
            }
            Console.Write(line);

            for(int i = 0; i < relativePos + line.lineNumber.ToString().Length; i++)
                Console.Write(" ");
            for (int i = 0; i < diagnostic.location.len; i++)
                Console.Write("~");

            Console.WriteLine();
            Console.WriteLine(diagnostic.GetMessage());
            Console.WriteLine();
        }
    }
}
