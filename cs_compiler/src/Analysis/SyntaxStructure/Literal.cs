using System.Diagnostics;
using Nyx.Symbols;

namespace Nyx.Analysis;

public class Literal : Expression
{
    public string name { get; }
    public override TypeSymbol? typeSymbol => throw new NotImplementedException();
    
    public Literal(LexerNode name) : base(name.location)
    {
        Debug.Assert(name.value != null);
        this.name = name.value;
    }
}