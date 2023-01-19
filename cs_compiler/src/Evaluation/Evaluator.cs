using Nyx.Analysis;
using Nyx.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Evaluation;

public class Evaluator
{
    private String _input;
    private SyntaxNode _program;
    private Dictionary<FunctionSymbol, SyntaxNode> _functions;
    private Stack<Dictionary<FunctionSymbol, object>> _locals;
    private DiagnosticCollection _diagnostics;

    public Evaluator(String input, SyntaxNode compilationUnit)
    {
        _input = input;
        _program = compilationUnit;
        _functions = new Dictionary<FunctionSymbol, SyntaxNode>();
        _locals = new Stack<Dictionary<FunctionSymbol, object>>();
        _locals.Push(new Dictionary<FunctionSymbol, object>());
        _diagnostics = new DiagnosticCollection();
    }

    private String _GetString(TextLocation location)
    {
        return _input.Substring(location.pos, location.len);
    }

    private SymbolModifiers _GetModifiers(SyntaxNode modifiers)
    {
        if (modifiers.kind != SyntaxKind.Syntax_Modifiers)
            throw new Exception("These must be modifiers.");

        return new SymbolModifiers(modifiers.children[1].kind != SyntaxKind.Syntax_Empty);
    }

    private TypeSymbol _GetType(SyntaxNode identifier)
    {
        if (identifier.kind != SyntaxKind.Token_Literal)
            throw new Exception("This must be a identifier.");

        var str = _GetString(identifier.location);
        switch(str)
        {
            case "void": return TypeSymbol.Void;
            case "bool": return TypeSymbol.Bool;
            case "i32": return TypeSymbol.I32;
            default:
                throw new Exception($"This type is not supported ({str})");
        }
    }

    private List<VariableSymbol> _GetFunctionParams(SyntaxNode param)
    {
        if (param.kind != SyntaxKind.Syntax_Parameters)
            throw new Exception("These must be parameters.");
        
        var symbols = new List<VariableSymbol>();

        if (param.children[0].kind == SyntaxKind.Syntax_Empty)
            return symbols;
        
        for (int i = 0; i < param.children.Count - 1; i += 2)
        {
            var p = param.children[i];
            symbols.Add(new VariableSymbol(
                _GetString(p.children[2].location),
                _GetModifiers(p.children[0]),
                _GetType(p.children[3].children[1])
            ));
        }

        return symbols;
    }

    public void Evaluate()
    {
        if (_program.kind != SyntaxKind.Syntax_CompilationUnit)
            return;

        foreach(var item in _program.children)
        {
            switch(item.kind)
            {
                case SyntaxKind.Syntax_FunctionImplementation:
                    _functions.Add(new FunctionSymbol
                    (
                        _GetString(item.children[2].location), 
                        _GetModifiers(item.children[0]),
                        _GetFunctionParams(item.children[4]),
                        TypeSymbol.Void
                    ), item.children[7]);
                    break;
                case SyntaxKind.Token_End:
                    break;
                default:
                    throw new Exception($"Error durring runtime: <{item.kind}> is not allowed as topLevelItem!");
            }
        }

        foreach(var f in _functions.Keys)
        {
            if (f.name == "main")
            {
                _EvaluateFunction(f);
                break;
            }
        }
    }

    private void _EvaluateFunction(FunctionSymbol function)
    {

    }
}