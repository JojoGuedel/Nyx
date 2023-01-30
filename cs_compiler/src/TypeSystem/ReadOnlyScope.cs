using Nyx.Analysis;

namespace Nyx.TypeSystem;

public class ReadOnlyScope
{
    protected Dictionary<string, Symbol> _scope;

    public ReadOnlyScope()
    {
        _scope = new Dictionary<string, Symbol>();
    }

    public Symbol? TryGet(Identifier identifier) => TryGet(identifier.name);
    public Symbol? TryGet(string name)
    {
        if (_scope.TryGetValue(name, out var symbol))
            return symbol;

        return null;
    }
}