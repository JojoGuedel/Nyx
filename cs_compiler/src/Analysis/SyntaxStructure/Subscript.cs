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
}