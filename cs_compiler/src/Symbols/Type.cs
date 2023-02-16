using Nyx.Analysis;

namespace Nyx.Symbols;

public class Type : Symbol
{
    public override Scope scope { get; }

    public Type(string name) : base(name) 
    { 
        scope = new Scope();
    }
}
