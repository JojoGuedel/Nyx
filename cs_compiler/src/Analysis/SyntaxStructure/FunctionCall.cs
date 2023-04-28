using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class FunctionCall : _Expression
{
    public _Expression expression { get; }
    // TODO: optional arguments
    public ImmutableArray<_Expression> arguments { get; }

    public FunctionCall(_Expression expression, LexerNode lParen, ImmutableArray<_Expression> arguments, LexerNode rParen) : 
        base(TextLocation.Embrace(expression.location, rParen.location))
    {
        this.expression = expression;
        this.arguments = arguments;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "FunctionCall");
        indent += _ChildIndent(isLast);
        expression.Write(writer, indent, false);
        _WriteArray(writer, indent, true, "arguments", Array.ConvertAll(arguments.ToArray(), (_Expression statement) => (_Node)statement));
    }
}