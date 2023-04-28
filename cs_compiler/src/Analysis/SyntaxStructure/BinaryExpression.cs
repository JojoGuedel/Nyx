using Nyx.Utils;

namespace Nyx.Analysis;

public class BinaryExpression : _Expression
{
    public _Expression left { get; }
    public LexerNode @operator { get; }
    public _Expression right { get; }

    public BinaryExpression(_Expression left, LexerNode opeartor, _Expression right) : 
        base(TextLocation.Embrace(left.location, right.location))
    {
        this.left = left;
        this.@operator = opeartor;
        this.right = right;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "BinaryExpression");
        indent += _ChildIndent(isLast);
        left.Write(writer, indent, false);
        @operator.Write(writer, indent, false);
        right.Write(writer, indent, true);
    }
}
