using System.Collections.Immutable;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Subscript : Expression
{
    public Expression expression { get; }
    public ImmutableArray<Expression> arguments { get; }
    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public Subscript(Expression expression, LexerNode lSquare, ImmutableArray<Expression> arguments, LexerNode rSquare) : 
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
        _WriteArray(writer, indent, true, "arguments", Array.ConvertAll(arguments.ToArray(), (Expression statement) => (Node)statement));
    }
}