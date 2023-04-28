using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Subscript : _Expression
{
    public _Expression expression { get; }
    public ImmutableArray<_Expression> arguments { get; }

    public Subscript(_Expression expression, LexerNode lSquare, ImmutableArray<_Expression> arguments, LexerNode rSquare) : 
        base(TextLocation.Embrace(expression.location, rSquare.location))
    {
        this.expression = expression;
        this.arguments = arguments;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Subscript");
        indent += _ChildIndent(isLast);
        expression.Write(writer, indent, false);
        _WriteArray(writer, indent, true, "arguments", Array.ConvertAll(arguments.ToArray(), (_Expression statement) => (_Node)statement));
    }
}