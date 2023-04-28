using Nyx.Utils;

namespace Nyx.Analysis;

public class Prefix : _Expression
{
    public LexerNode prefix { get; }
    public _Expression expression { get; }

    public Prefix(LexerNode prefix, _Expression expression) : 
        base(TextLocation.Embrace(prefix.location, expression.location))
    {
        this.prefix = prefix;
        this.expression = expression;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Prefix");
        indent += _ChildIndent(isLast);
        prefix.Write(writer, indent, false);
        expression.Write(writer, indent, true);
    }
}