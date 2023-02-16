using Nyx.Analysis;

namespace Nyx.Symbols;

public abstract class Symbol
{
    public string name { get; }
    public abstract ReadonlyScope? scope { get; }

    public Symbol(string name)
    {
        this.name = name;
    }
}
