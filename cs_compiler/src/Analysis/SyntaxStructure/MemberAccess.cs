using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class MemberAccess : Expression
{
    public Expression expression { get; }
    public Expression name { get; }
    public override TypeSymbol? typeSymbol => throw new NotImplementedException();
    
    public MemberAccess(Expression expression, LexerNode dot, Expression name) : 
        base(TextLocation.Embrace(expression.location, name.location))
    {
        this.expression = expression;
        this.name = name;
    }
}
