using System.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Prefix : Expression
{
    public SyntaxKind prefix { get; }
    public Expression expression { get; }

    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public Prefix(LexerNode prefix, Expression expression) : 
        base(TextLocation.Embrace(prefix.location, expression.location))
    {
        this.prefix = prefix.kind;
        this.expression = expression;
    }
}