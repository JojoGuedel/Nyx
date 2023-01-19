using System.Collections.Immutable;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class FunctionCall : Expression
{
    public Expression expression { get; }
    // TODO: optional arguments
    public ImmutableArray<Expression> arguments { get; }
    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public FunctionCall(Expression expression, LexerNode lParen, ImmutableArray<Expression> arguments, LexerNode rParen) : 
        base(TextLocation.Embrace(expression.location, rParen.location))
    {
        this.expression = expression;
        this.arguments = arguments;
    }
}