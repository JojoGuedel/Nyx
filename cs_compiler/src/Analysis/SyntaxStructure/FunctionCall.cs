using System.Collections.Immutable;

namespace Nyx.Analysis;

internal class FunctionCall : Expression
{
    internal override Location location { get; }
    public Expression expression { get; }
    // TODO: optional arguments
    public ImmutableArray<Expression> arguments { get; }

    internal FunctionCall(
        Expression expression, 
        Token lParen, 
        ImmutableArray<Expression> arguments, 
        Token rParen)
    {
        location = Location.Embrace(expression.location, rParen.location);
        this.expression = expression;
        this.arguments = arguments;
    }
}