using Nyx.Analysis;

namespace Nyx.TypeSystem;

public class SymbolEnvironment
{
    Stack<Scope> _scopes;
    Scope _current { get => _scopes.Peek(); }
    public bool isGlobal { get => _scopes.Count == 1; }

    public SymbolEnvironment()
    {
        _scopes = new Stack<Scope>();
        EnterNewScope();
    }

    public void EnterNewScope()
    {
        _scopes.Push(new Scope());
    }

    public void ExitCurrentScope()
    {
        if (!isGlobal)
            _scopes.Pop();
    }

    public void EnterScope(Namespace @namespace)
    {
        if (_current.TryGet(@namespace.name) is Namespace)
            _scopes.Push(@namespace.members);
    }

    public void EnterScope(TypeSystem.Symbol @object)
    {
        if (_current.TryGet(@object.name) is TypeSystem.Symbol)
            _scopes.Push(@object.members);
    }

    public Symbol? TryGet(Identifier identifier) => _current.TryGet(identifier);
    public Symbol? TryGet(string name) => _current.TryGet(name);

    public Symbol? TryGet(MemberAccess memberAccess)
    {
        // TODO: implement this
        throw new NotImplementedException();
    }
    
    public Symbol? TryGet(Expression expression)
    {
        switch(expression)
        {
            case Identifier identifier: 
                return TryGet(identifier);
            case MemberAccess memberAccess: 
                return TryGet(memberAccess);
            default: 
                return null;
        }
    }

    public bool TryDeclare(Symbol symbol) => _current.TryDeclare(symbol);
}
