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

        var ret = new SyntaxNode(kinds[0], _currentToken.location);
        diagnostics.Add(new Error_UnexpectedToken(_currentToken, kinds));
        _IncrementPos();
        return ret;
    }

    private SyntaxNode _ParseCompilationUnit()
    {
        List<SyntaxNode> syntaxNodes = new List<SyntaxNode>();

        while (_currentToken.kind != SyntaxKind.Token_End)
            syntaxNodes.Add(_ParseDeclarationStatement());
        syntaxNodes.Add(_currentToken);

        return new SyntaxNode(SyntaxKind.Syntax_CompilationUnit, syntaxNodes);
    }

    private SyntaxNode _ParseFunctionDeclaration()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_FunctionDeclaration,
            _ParseOptionalPublicKeyword(),
            _ParseOptionalStaticKeyword(),
            _MatchToken(SyntaxKind.Keyword_Function),
            _MatchToken(SyntaxKind.Token_LParen),
            _ParseOptionalArguments(),
            _MatchToken(SyntaxKind.Token_RParen),
            _ParseOptionalReturnType(),
            _MatchToken(SyntaxKind.Token_Colon),
            _MatchToken(SyntaxKind.Token_BeginBlock),
            _ParseFunctionBody(),
            _MatchToken(SyntaxKind.Token_EndBlock)
        );
    }

    private SyntaxNode _ParseOptionalPublicKeyword()
    {
        if (_currentToken.kind == SyntaxKind.Keyword_Public)
            return _ReadNext();
        
        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    private SyntaxNode _ParseOptionalStaticKeyword()
    {
        if (_currentToken.kind == SyntaxKind.Keyword_Public)
            return _ReadNext();
        
        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    private SyntaxNode _ParseOptionalArguments()
    {
        var arguments = new List<SyntaxNode>();
        while (_currentToken.kind != SyntaxKind.Token_RParen)
            arguments.Add(_ParseArgument());

        if (arguments.Count == 0)
            return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);

        return new SyntaxNode(SyntaxKind.Syntax_Arguments, arguments);
    }

    private SyntaxNode _ParseArgument()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Argument,
            _MatchToken(SyntaxKind.Keyword_Var),
            _MatchToken(SyntaxKind.Token_Identifier),
            // TODO: maybe remove this
            _ParseOptionalTypeClause()
        );
    }

    private SyntaxNode _ParseOptionalReturnType()
    {
        if (_currentToken.kind == SyntaxKind.Token_RArrow)
            return _ParseReturnType();
        
        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    private SyntaxNode _ParseReturnType()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ReturnType,
            _MatchToken(SyntaxKind.Token_RArrow),
            _MatchToken(SyntaxKind.Token_Identifier)
        );
    }

    private SyntaxNode _ParseFunctionBody()
    {
        var children = new List<SyntaxNode>();

        if (_currentToken.kind == SyntaxKind.Token_EndBlock)
            diagnostics.Add(new Error_EmptyBlock(_currentToken.location));
        else
            while(_currentToken.kind != SyntaxKind.Token_EndBlock)
                children.Add(_ParseStatement());

        return new SyntaxNode
        (
            SyntaxKind.Syntax_FunctionBlock,
            children
        );
    }

    private SyntaxNode _ParseStatement()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Semicolon:
                break;

            case SyntaxKind.Keyword_Function:
                return _ParseFunctionDeclaration();
            case SyntaxKind.Keyword_Return:
                return _ParseReturnStatement();
            case SyntaxKind.Keyword_Pass:
                break;
            case SyntaxKind.Keyword_Mut:
            case SyntaxKind.Keyword_Var:
                return _ParseDeclarationStatement();
            case SyntaxKind.Keyword_If:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Else:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Switch:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_For:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Do:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_While:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Continue:
                return _ParseContinueStatement();
            case SyntaxKind.Keyword_Break:
                return _ParseBreakStatement();
        }

        diagnostics.Add(new Error_InvalidStatement(_currentToken));
        return new SyntaxNode(SyntaxKind.Syntax_Error, _ReadNext());
    }

    private SyntaxNode _ParseReturnStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ReturnStatement,
            _MatchToken(SyntaxKind.Keyword_Return),
            _ParseOptionalReturnExpression(),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseOptionalReturnExpression()
    {
        if (_currentToken.kind == SyntaxKind.Token_Semicolon)
            return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
        
        return _ParseExpression();
    }

    private SyntaxNode _ParsePassStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_PassStatement,
            _MatchToken(SyntaxKind.Syntax_PassStatement),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseContinueStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ContinueStatement,
            _MatchToken(SyntaxKind.Keyword_Continue),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseBreakStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_BreakStatement,
            _MatchToken(SyntaxKind.Keyword_Break),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseDeclarationStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_DeclarationStatement,
            _MatchToken(SyntaxKind.Keyword_Var),
            _MatchToken(SyntaxKind.Token_Identifier),
            _ParseOptionalTypeClause(),
            _MatchToken(SyntaxKind.Token_Equal),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseOptionalTypeClause()
    {
        if (_currentToken.kind == SyntaxKind.Token_Colon)
            return _ParseTypeClause();

        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    private SyntaxNode _ParseTypeClause()
    {
        var token_Colon = _MatchToken(SyntaxKind.Token_Colon);
        var token_Identifier = _MatchToken(SyntaxKind.Token_Identifier);

        return new SyntaxNode(SyntaxKind.Syntax_TypeClause, token_Colon, token_Identifier);
    }

    private SyntaxNode _ParseExpression()
    {
        return _ParseSum();
    }

    private SyntaxNode _ParseSum()
    {
        var left = _ParseMultiplication();

        while (true)
        {            
            if (_currentToken.kind == SyntaxKind.Token_Plus)
                left = new SyntaxNode(SyntaxKind.Syntax_Addition, left, _ReadNext(), _ParseMultiplication());
            else if (_currentToken.kind == SyntaxKind.Token_Minus)
                left = new SyntaxNode(SyntaxKind.Syntax_Subtraction, left, _ReadNext(), _ParseMultiplication());
            else
                break;
        }

        return left;
    }

    private SyntaxNode _ParseMultiplication()
    {
        var left = _ParsePrimary();

        while (true)
        {
            if (_currentToken.kind == SyntaxKind.Token_Star)
                left = new SyntaxNode(SyntaxKind.Syntax_Multiplication, left, _ReadNext(), _ParsePrimary());
            else if (_currentToken.kind == SyntaxKind.Token_Slash)
                left = new SyntaxNode(SyntaxKind.Syntax_Division, left, _ReadNext(), _ParsePrimary());
            else
                break;
        }

        return left;
    }

    private SyntaxNode _ParsePrimary()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Identifier:
            case SyntaxKind.Token_Number:
            case SyntaxKind.Token_String:
                return _ReadNext();

            default:
                return new SyntaxNode(SyntaxKind.Syntax_Error, _currentToken.location);
        }
    }
}
