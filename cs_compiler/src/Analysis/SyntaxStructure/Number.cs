using System.Diagnostics;

namespace Nyx.Analysis;

public class Number : Expression
{
    public string value { get; }

    public Number(LexerNode number) : base(number.location)
    {
        Debug.Assert(number.value != null);
        
        value = number.value;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Number");
        indent += _ChildIndent(isLast);
        _WriteString(writer, indent, true, value);
    }
}