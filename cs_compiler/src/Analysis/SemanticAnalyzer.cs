using System.Collections.Immutable;
using Nyx.TypeSystem;

namespace Nyx.Analysis;

public class SemanticAnalyzer
{
    CompilationUnit _compilationUnit;
    SymbolEnvironment _environment;

    public SemanticAnalyzer(CompilationUnit compilationUnit) 
    {
        _compilationUnit = compilationUnit;
        _environment = new SymbolEnvironment();
    }

    public void Bind()
    {
        // TODO: bind objects

        var globalFuncs = _compilationUnit.members.Where((member) => member is Function func && func.modifiers.global is not EmptyNode);
        var globalFuncDecls = new List<FunctionSymbol>();

        foreach(var func in globalFuncs)
        {
            _BindGlobalFuncDecl((Function)func);
            globalFuncDecls.Add(func.symbol);
        }
    }

    void _BindGlobalFuncDecl(Function function)
    {
        _BindTypeClause(function.type);
        _BindModifiers(function.modifiers);
        _BindParameters(function.parameters);

        Function.TryAssignSymbol()

        var symbol = new FunctionSymbol
        (
            boundName,
            boundType,
            boundMods,
            boundParams
        );

        if(!_environment.TryDeclare(symbol))
            // TODO: diagnostics
            throw new NotImplementedException();

        return new BoundFunctionDeclaration();
    }

    void _BindTypeClause(TypeClause typeClause)
    {
        if(_environment.TryGet(typeClause.type) is not TypeSystem.Symbol type)
            // TODO: diagnostics
            throw new NotImplementedException();
        
        if (!typeClause.TryAssignSymbol(type))
            // TODO: diagnostics
            throw new NotImplementedException();
    }

    BoundModifiers _BindModifiers(Modifiers modifiers) => new BoundModifiers
    (
        modifiers.global is not EmptyNode,
        modifiers.@static is not EmptyNode,
        modifiers.mutable is not EmptyNode
    );

    BoundParameters _BindParameters(ImmutableArray<Parameter> parameters)
    {
        
    }

    BoundParameters _BindParameter(Parameter parameter)
    {

    }

    void _BindFunction()
    {

    }

    void _BindExpression()
    {

    }

    void _BindMemberAccess(MemberAccess memberAccess, Dictionary<string, Symbol> scope = _scopes)
    {

    }
}

public class BoundModifiers
{
    public bool isGlobal { get; }
    public bool isStatic { get; }
    public bool isMutable { get; }

    public BoundModifiers(bool isGlobal, bool isStatic, bool isMutable)
    {
        this.isGlobal = isGlobal;
        this.isStatic = isStatic;
        this.isMutable = isMutable;
    }
}