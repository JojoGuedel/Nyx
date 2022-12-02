using Diagnostics;

namespace CodeAnalysis;

class SyntaxAnalyzer : AAnalyzer<SyntaxNode, SyntaxNode>
{
    public DiagnosticCollection diagnostics;
    SyntaxDefinition _syntax;
    SyntaxNode _currentToken { get => _Peek(0); }
    int _currentEmptySyntaxPos { get => _currentToken.location.pos + _currentToken.location.len; }
    bool _isCurrentValid;

    public SyntaxAnalyzer(SyntaxDefinition syntax, List<SyntaxNode> tokens) : base(tokens, tokens.Last())
    {
        diagnostics = new DiagnosticCollection();
        _syntax = syntax;
        _isCurrentValid = true;
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        yield return _ParseCompilationUnit();
    }

    private SyntaxNode _MatchToken(params SyntaxKind[] kinds)
    {
        foreach (var kind in kinds)
            if (_currentToken.kind == kind) return _ReadNext();

        // TODO: diagnostics
        throw new NotImplementedException();

        // var ret = new SyntaxNode(kinds[0], _currentToken.location);
        // diagnostics.Add(new Error_UnexpectedToken(_currentToken, kinds));
        // _IncrementPos();
        // return ret;
    }

    private SyntaxNode _ParseCompilationUnit()
    {
        List<SyntaxNode> children = new List<SyntaxNode>();

        while (_currentToken.kind != SyntaxKind.Token_End)
            children.Add(_ParseTopLevelItem());
        children.Add(_currentToken);

        return new SyntaxNode(SyntaxKind.Syntax_CompilationUnit, children);
    }

    private SyntaxNode _ParseTopLevelItem()
    {
        switch(_currentToken.kind)
        {
            case SyntaxKind.Keyword_Public:
            case SyntaxKind.Keyword_Static:
            case SyntaxKind.Keyword_Abstract:
            case SyntaxKind.Keyword_Enum:
            case SyntaxKind.Keyword_Struct:
            case SyntaxKind.Keyword_Function:
                return _ParseTopLevelMember();
            case SyntaxKind.Keyword_Section:
                return _ParseSection();
            case SyntaxKind.Keyword_Include:
                return _ParseIncludeStatement();
            default:
                // TODO: diagnostics
                throw new NotImplementedException();
                // diagnostics.Add(new Error_InvalidTopLevelItem(_currentToken));
                // return new SyntaxNode(SyntaxKind.Syntax_Error, _ReadNext());
        }
    }

    // ----------------------------- _ParseTopLevelItem -----------------------------
    private SyntaxNode _ParseTopLevelMember()
    {
        var syntaxPublic = _ParseOptionalPublic(); 
        var syntaxAbstract = _ParseOptionalAbstract(); 
        var syntaxStatic = _ParseOptinoalStatic(); 

        switch(_currentToken.kind)
        {
            case SyntaxKind.Keyword_Enum:
                // TODO: parse enums
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Struct:
                return _ParseStructDefinition(syntaxPublic, syntaxAbstract, syntaxStatic);
            case SyntaxKind.Keyword_Function:
                return _ParseFunctionImplementation(syntaxPublic, syntaxAbstract, syntaxStatic);
            default:
                // TODO: diagnostics
                throw new NotImplementedException();
        }
    }

    private SyntaxNode _ParseSection()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Section,
            _MatchToken(SyntaxKind.Keyword_Section),
            _MatchToken(SyntaxKind.Token_Identifier),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseIncludeStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_IncludeStatement,
            _MatchToken(SyntaxKind.Keyword_Include),
            // TODO: resolve name
            _MatchToken(SyntaxKind.Token_Identifier),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }
    // ----------------------------- _ParseTopLevelItem -----------------------------

    // ---------------------------- _ParseTopLevelMember ----------------------------
    private SyntaxNode _ParseOptionalPublic()
    {
        if (_currentToken.kind == SyntaxKind.Keyword_Public)
            return _MatchToken(SyntaxKind.Keyword_Public);
        
        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }
    private SyntaxNode _ParseOptionalAbstract()
    {
        if (_currentToken.kind == SyntaxKind.Keyword_Abstract)
            return _MatchToken(SyntaxKind.Keyword_Abstract);
        
        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    private SyntaxNode _ParseOptinoalStatic()
    {
        if (_currentToken.kind == SyntaxKind.Keyword_Static)
            return _MatchToken(SyntaxKind.Keyword_Static);
        
        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    private SyntaxNode _ParseStructDefinition(SyntaxNode syntaxPublic, SyntaxNode syntaxAbstract, SyntaxNode syntaxStatic)
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_StructDefinition,
            syntaxPublic,
            syntaxAbstract,
            syntaxStatic,
            _MatchToken(SyntaxKind.Token_Identifier),
            _MatchToken(SyntaxKind.Token_Colon)
            // TODO: parse struct body
        );
    }

    private SyntaxNode _ParseFunctionImplementation(SyntaxNode syntaxPublic, SyntaxNode syntaxAbstract, SyntaxNode syntaxStatic)
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_FunctionDeclaration,
            syntaxPublic,
            syntaxAbstract,
            syntaxStatic,
            _MatchToken(SyntaxKind.Token_LParen),
            // TODO: parse optional args
            _MatchToken(SyntaxKind.Token_RParen),
            // TODO: parse optional type clause
            _MatchToken(SyntaxKind.Token_Colon)
            // TODO: parse function body
        );
    }
    // ---------------------------- _ParseTopLevelMember ----------------------------

    private SyntaxNode _ParseExpression()
    {
        var left = _ParsePrefix();

        

        return left;
    }

    private SyntaxNode _ParsePrefix()
    {
        var syntaxKind = SyntaxKind.Syntax_Empty;

        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Plus:
                syntaxKind = SyntaxKind.Syntax_PrefixPlus;
                break;
            case SyntaxKind.Token_Minus:
                syntaxKind = SyntaxKind.Syntax_PrefixMinus;
                break;
            case SyntaxKind.Keyword_Not:
                syntaxKind = SyntaxKind.Syntax_PrefixNot;
                break;
            case SyntaxKind.Token_PlusPlus:
                syntaxKind = SyntaxKind.Syntax_PrefixPlusPlus;
                break;
            case SyntaxKind.Token_MinusMinus:
                syntaxKind = SyntaxKind.Syntax_PrefixMinusMinus;
                break;
        }

        if (syntaxKind != SyntaxKind.Syntax_Empty)
            return new SyntaxNode
            (
                syntaxKind,
                _ReadNext(),
                _ParsePrefix()
            );

        return _ParsePostfix();
    }

    // -------------------------------- _ParsePrefix --------------------------------
    private SyntaxNode _ParsePostfix()
    {
        var expr = _ParsePrimary();
        var isPostfix = true;

        while(isPostfix)
        {
            switch(_currentToken.kind)
            {
                case SyntaxKind.Token_LParen:
                    throw new NotImplementedException();
                case SyntaxKind.Token_PlusPlus:
                case SyntaxKind.Token_MinusMinus:
                    throw new NotImplementedException();
                default:
                    isPostfix = false;
                    break;
            }
        }

        return expr;
    }
    // -------------------------------- _ParsePrefix --------------------------------

    // -------------------------------- _ParsePostfix -------------------------------
    private SyntaxNode _ParsePrimary()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Number:
            case SyntaxKind.Token_String:
            case SyntaxKind.Token_Identifier:
                return _ReadNext();
        }

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Error,
            _ReadNext()
        );
    }
    // -------------------------------- _ParsePostfix -------------------------------
}