using System.Diagnostics;
using Nyx.Symbols;

namespace Nyx.Analysis;

public class Identifier : Expression
{
    public string name { get; }
    public override TypeSymbol? typeSymbol => throw new NotImplementedException();
    
    public Identifier(LexerNode name) : base(name.location)
    {
        Debug.Assert(name.value != null);
        this.name = name.value;
    }
}