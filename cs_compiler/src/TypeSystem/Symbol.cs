using System.Diagnostics;

namespace Nyx.TypeSystem;

// public abstract class Symbol
// {
//     public string name { get; }
//     public abstract ReadOnlyScope? members { get; }

//     public Symbol(string name)
//     {
//         this.name = name;
//     }
// }

public class Name
{
    public string[] chain { get; }
    public string full { get => string.Join(".", chain); }
    public bool isChained { get => chain.Length > 1; }

    public Name(string name)
    {
        // TODO: parse identifiers differently
        chain = name.Split('.');
    }
}

public class Environment
{
    ScopeSymbol _current;

    public Environment()
    {
        _current = new GlobalScope();
    }

    public void ExitAll()
    {
        while(_current.parent is not null)
            _current = _current.parent;
    }

    public void EnterNamespace(Name name)
    {
        throw new NotImplementedException();
    }

    public void EnterNewScope()
    {
        throw new NotImplementedException();
    }

    // TODO: return more information to explain, what went wrong (instead of bool)
    public bool DeclObject(Name name)
    {
        if (name.isChained)
            return false;
        
        throw new NotImplementedException();
    }

    public bool DeclFunction(Name name)
    {
        if (name.isChained)
            return false;
        
        throw new NotImplementedException();
    }

    public bool ImplFunction(Name name)
    {
        throw new NotImplementedException();
    }

    public bool DeclValue(Name name)
    {
        if (name.isChained)
            return false;

        throw new NotImplementedException();
    }

    public bool AssignValue(Name name)
    {
        throw new NotImplementedException();
    }

    // public void EnterScope(Name name)
    // {
    //     Debug.Assert(!name.isChained);

    //     _current.LookupSymbol(name);
    //     _scopes.Push(scope);
    // }
}

public class Namespace : Symbol
{
    public Namespace(ScopeSymbol? parent) : base(parent) { }

    bool _DeclObject() 
    {
        throw new NotImplementedException();
    }

    bool _DeclFunction() 
    {
        throw new NotImplementedException();
    }

    public override Symbol? LookupSymbol(Name name)
    {
        throw new NotImplementedException();
    }

    public override bool DeclareSymbol(Symbol symbol)
    {
        if (symbol is )
        throw new NotImplementedException();
    }
}

public class Scope
{
    Scope? _parent;
    Dictionary<string, Symbol> _scope;

    public Scope()
    {
        _scope = new Dictionary<string, Symbol>();
    }

    public Symbol? LookupSymbol(Name name)
    {
        var current = name.chain[0];

        if (_parent is null)
            return _LookupSymbol(name, 0);
        
        if (!_scope.ContainsKey(name.chain[0]))
            ;

        return 
    }

    public bool DeclareSymbol(Symbol symbol)
    {
        return _scope.TryAdd(symbol.name, symbol);
    }

    Symbol? _LookupSymbol(Name name, int index)
    {
        if ()

        if (index >= name.chain.Length)
            ;
    }
}

public abstract class ScopeSymbol : Symbol
{
    public ScopeSymbol? parent { get; }
    public Dictionary<string, Symbol> scope;

    public ScopeSymbol(ScopeSymbol? parent)
    {
        this.parent = parent;
        scope = new Dictionary<string, Symbol>();
    }    

    public abstract bool DeclareSymbol(Symbol symbol);
}

public class ExecScope : ScopeSymbol
{
    Dictionary<string, Variable> _variables;

    public bool DeclVariable() 
    {
        throw new NotImplementedException();
    }

    public override Symbol LookupSymbol(Name name)
    {
        throw new NotImplementedException();
    }
}

public class ObjectScope : Scope
{
    Dictionary<string, Variable> _variables;
    Dictionary<string, Function> _functions;

    public bool DeclVariable() 
    {
        throw new NotImplementedException();
    }

    public bool DeclFunction() 
    {
        throw new NotImplementedException();
    }
}

public class GlobalScope : Namespace
{
    // TODO: maybe allow global variables?
    Dictionary<string, Function> _functions;

    public bool DeclNamespace() 
    {
        throw new NotImplementedException();
    }
}

public abstract class Declaration
{

}

public class ObjectDecl : Declaration
{
    string name;

    // List<TraitDeclaration> traits;
    Namespace declaration;
}

public class Parameters
{
    List<VariableDecl> parameters;
}

public class FunctionDecl : Declaration
{
    string name;

    Modifiers modifiers;
    Parameters parameters;
    Object type;
}

public class PropertyDecl : Declaration
{
    string name;

    Modifiers modifiers;
    Object type;
}

public class VariableDecl : Declaration
{
    string name;

    Modifiers modifiers;
    Object type;
}

public abstract class Symbol
{
    public string name { get; }
    // public abstract Declaration declaration { get; }
}

public class Function : Symbol
{
    // public override FunctionDecl declaration => throw new NotImplementedException();

    ExecScope? implementation;
}

public class Object : Symbol
{
    // public override ObjectDecl declaration => throw new NotImplementedException();
}

public class Property : Symbol
{
    // public override PropertyDecl declaration => throw new NotImplementedException();
}

public class Variable : Symbol
{
    // public override VariableDecl declaration => throw new NotImplementedException();
}

public class Modifiers
{
    public bool isStatic { get; }

    public bool isMutable { get; }
    public bool isPubMutable { get; }

    public bool isReadable { get; }
    public bool isPubReadable { get; }
}