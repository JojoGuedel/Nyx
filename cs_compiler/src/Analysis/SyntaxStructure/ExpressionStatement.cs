using System.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class ExpressionStatement : Statement
{
    Expression expression;

    public ExpressionStatement(Expression expression, LexerNode semicolon) : 
        base(TextLocation.Embrace(expression.location, semicolon.location))
    {
        this.expression = expression;
    }
}

public class ReturnStatement : Statement
{
    public Expression expression { get; }

    public ReturnStatement(LexerNode @return, Expression expression, LexerNode semicolon) : 
        base(TextLocation.Embrace(@return.location, semicolon.location))
    {
        this.expression = expression;
    }
}