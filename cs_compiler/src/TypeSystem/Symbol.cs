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

public class ScopeStack
{
    // TODO: reduce complexity???
    Scope globalScope;
    Stack<StructScope> structScopes;
    Stack<ObjectScope> objectScopes;
    Stack<ExecScope> execScopes;
}

public class Environment
{
    Stack<Scope> _scopes;
    Scope _current { get => _scopes.Peek(); }
    
    public GlobalScope globalScope => (GlobalScope)_scopes.Last();

    public Environment()
    {
        _scopes = new Stack<Scope>();
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

public abstract class Scope
{
    public abstract Symbol LookupSymbol(Name name);
}

public class ExecScope : Scope
{
    Dictionary<string, Variable> _variables;

    public bool DeclVariable() 
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

public class StructScope : Scope
{
    Dictionary<string, StructScope> _namespaces;
    Dictionary<string, Object> _objects;

    public bool DeclObject() 
    {
        throw new NotImplementedException();
    }

    public bool DeclFunction() 
    {
        throw new NotImplementedException();
    }
}

public class GlobalScope : StructScope
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
    StructScope declaration;
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
    public abstract Declaration declaration { get; }
}

public class Function : Symbol
{
    public override FunctionDecl declaration => throw new NotImplementedException();

    ExecScope? implementation;
}

public class Object : Symbol
{
    public override ObjectDecl declaration => throw new NotImplementedException();
}

public class Property : Symbol
{
    public override PropertyDecl declaration => throw new NotImplementedException();
}

public class Variable : Symbol
{
    public override VariableDecl declaration => throw new NotImplementedException();
}

public class Modifiers
{
    public bool isStatic { get; }

    public bool isMutable { get; }
    public bool isPubMutable { get; }

    public bool isReadable { get; }
    public bool isPubReadable { get; }
}