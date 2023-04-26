using Nyx.Analysis;

namespace Nyx.Symbols;

public abstract class Symbol
{
    public string name { get; }
    public string fullName { get; }

    public abstract IReadonlyScope? scope { get; }

    public Symbol(string name, Symbol? parent)
    {
        this.name = name;
        fullName = string.Empty;

        if (parent is not null)
            fullName += parent.fullName + ".";
        
        fullName += this.name;
    }
}
