using System.Collections.Immutable;
using Nyx.Symbols;

namespace Nyx.Analysis;

public class ReadonlyScope
{
    protected Dictionary<string, Symbol> _scope;

    public ReadonlyScope()
    {
        _scope = new Dictionary<string, Symbol>();
    }

    public Symbol? LookupSymbol(string name)
    {
        return _scope.GetValueOrDefault(name);
    }
}

public class 
Scope : ReadonlyScope
{
    public bool DeclareSymbol(Symbol symbol)
    {
        return _scope.TryAdd(symbol.name, symbol);
    }
}

public class ReadonlyEnvironment
{
    protected Scope _scope;

    public ReadonlyEnvironment()
    {
        _scope = new Scope();
    }

    public Symbol? LookupSymbol(string name)
    {
        return _scope.LookupSymbol(name);
    }
}

public class Environment : ReadonlyEnvironment
{
    public bool DeclareSymbol(Symbol symbol)
    {
        return _scope.DeclareSymbol(symbol);
    }
}

public class EnvironmentBuilder
{
    CompilationUnit _compilationUnit;

    public EnvironmentBuilder(CompilationUnit compilationUnit)
    {
        _compilationUnit = compilationUnit;
    }

    public void Build()
    {
        
    }
}
