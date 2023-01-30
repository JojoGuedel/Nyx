using System.Diagnostics;

namespace Nyx.Analysis;

public class String : Expression
{
    public string value { get; }

    public String(LexerNode @string) : 
        base(@string.location)
    {
        Debug.Assert(@string.value != null);

        value = @string.value;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "String");
        indent += _ChildIndent(isLast);
        _WriteString(writer, indent, true, value);
    }
}