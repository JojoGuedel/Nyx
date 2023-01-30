using Nyx.Utils;

namespace Nyx.Analysis;

public class Prefix : Expression
{
    public LexerNode prefix { get; }
    public Expression expression { get; }

    public Prefix(LexerNode prefix, Expression expression) : 
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