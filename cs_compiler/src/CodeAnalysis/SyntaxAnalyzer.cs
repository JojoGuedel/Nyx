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
        switch (_currentToken.kind)
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

        switch (_currentToken.kind)
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
        return _ParseBinaryExpression();
    }

    // ------------------------------ _ParseExpression ------------------------------
    private SyntaxNode _ParseBinaryExpression(int currentPrecedence = 1)
    {
        if (currentPrecedence > _syntax.maxOperatorPrecedence)
            return _ParsePrefix();

        var left = _ParseBinaryExpression(currentPrecedence + 1);

        while (_syntax.BinaryOperatorPrecedence(_currentToken.kind) == currentPrecedence)
        {
            left = new SyntaxNode
            (
                SyntaxKind.Syntax_BinaryOperation,
                left,
                _ReadNext(),
                _ParseBinaryExpression(currentPrecedence + 1)
            );
        }

        return left;
    }
    // ------------------------------ _ParseExpression ------------------------------

    // --------------------------- _ParseBinaryExpression ---------------------------
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

        if (syntaxKind == SyntaxKind.Syntax_Empty)
            return _ParsePostfix();

        return new SyntaxNode
        (
            syntaxKind,
            _ReadNext(),
            _ParsePrefix()
        );
    }
    // --------------------------- _ParseBinaryExpression ---------------------------

    // -------------------------------- _ParsePrefix --------------------------------
    private SyntaxNode _ParsePostfix()
    {
        var expr = _ParsePrimary();
        var isPostfix = true;

        while (isPostfix)
        {
            switch (_currentToken.kind)
            {
                case SyntaxKind.Token_LParen:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_FunctionCall,
                        expr,
                        _MatchToken(SyntaxKind.Token_LParen),
                        _ParseFunctionArguments(),
                        _MatchToken(SyntaxKind.Token_RParen)
                    );
                    break;
                case SyntaxKind.Token_LSquare:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_Subscript,
                        expr,
                        _MatchToken(SyntaxKind.Token_LSquare),
                        _ParseSubscriptArguments(),
                        _MatchToken(SyntaxKind.Token_RSquare)
                    );
                    break;
                case SyntaxKind.Token_PlusPlus:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_PostfixIncrement,
                        expr,
                        _ReadNext()
                    );
                    break;
                case SyntaxKind.Token_MinusMinus:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_PostfixDecrement,
                        expr,
                        _ReadNext()
                    );
                    break;
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

    // TODO: maybe merge this somehow with _ParseSubscriptArguments
    private SyntaxNode _ParseFunctionArguments()
    {
        var nodes = new List<SyntaxNode>();
        var isArgRemaining = _currentToken.kind != SyntaxKind.Token_RParen;

        while (isArgRemaining)
        {
            // TODO: diagnostics
            _ParseArgument();

            isArgRemaining = _currentToken.kind == SyntaxKind.Token_Comma;
            if (!isArgRemaining) break; 

            nodes.Add(_MatchToken(SyntaxKind.Token_Comma));
        }

        if (nodes.Count == 0)
            nodes.Add(SyntaxNode.EmptySyntax(_currentEmptySyntaxPos));

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Arguments,
            nodes
        );
    }

    private SyntaxNode _ParseSubscriptArguments()
    {
        var nodes = new List<SyntaxNode>();
        var isArgRemaining = _currentToken.kind != SyntaxKind.Token_RParen;

        while (isArgRemaining)
        {
            // TODO: diagnostics
            nodes.Add(new SyntaxNode(SyntaxKind.Syntax_Argument, _ParseExpression()));

            isArgRemaining = _currentToken.kind == SyntaxKind.Token_Comma;
            if (!isArgRemaining) break; 

            nodes.Add(_MatchToken(SyntaxKind.Token_Comma));
        }

        if (nodes.Count == 0)
            // TODO: diagnostics
            throw new NotImplementedException();

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Arguments,
            nodes
        );
    }
    // -------------------------------- _ParsePostfix -------------------------------

    // --------------------------- _ParseFunctionArguments --------------------------
    private SyntaxNode _ParseArgument()
    {
        var expr = _ParseExpression();

        if (expr.kind == SyntaxKind.Token_Identifier &&
            _currentToken.kind == SyntaxKind.Token_Equal)
            return new SyntaxNode
            (
                SyntaxKind.Syntax_OptionalArgument,
                expr,
                _ReadNext(),
                _ParseExpression()
            );
        
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Argument,
            expr
        );
    }
    // --------------------------- _ParseFunctionArguments --------------------------
}