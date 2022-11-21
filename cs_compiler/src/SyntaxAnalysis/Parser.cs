using Diagnostics;
using SyntaxAnalysis;

class Parser : AAnalyzer<SyntaxNode, SyntaxNode>
{
    public DiagnosticCollection diagnostics;
    SyntaxDefinition _syntax;
    SyntaxNode _currentToken { get => _Peek(0); }
    bool _isCurrentValid;

    public Parser(SyntaxDefinition syntax, List<SyntaxNode> tokens) : base(tokens, tokens.Last())
    {
        diagnostics = new DiagnosticCollection();
        _syntax = syntax;
        _isCurrentValid = true;
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        yield return GetNext();
    }

    public override SyntaxNode GetNext()
    {
        return _ParseCompilationUnit();
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

    private SyntaxNode _ParseDeclarationStatement()
    {
        var keyword_Let = _MatchToken(SyntaxKind.Keyword_Let);
        var token_Identifier = _MatchToken(SyntaxKind.Token_Identifier, SyntaxKind.Token_UnusedIdentifier);
        var syntax_OptionalTypeClause = _ParseOptionalTypeClause();
        var token_Equal = _MatchToken(SyntaxKind.Token_Equal);
        var syntax_Expression = _ParseExpression();
        var token_SemiColon = _MatchToken(SyntaxKind.Token_Semicolon);

        return new SyntaxNode(SyntaxKind.Syntax_DeclarationStatement,
                              keyword_Let,
                              token_Identifier,
                              syntax_OptionalTypeClause,
                              token_Equal,
                              syntax_Expression,
                              token_SemiColon);
    }

    private SyntaxNode _ParseOptionalTypeClause()
    {
        if (_currentToken.kind == SyntaxKind.Token_Colon)
            return _ParseTypeClause();

        return SyntaxNode.EmptySyntax(_currentToken.location.pos + _currentToken.location.len);
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
            if (_currentToken.kind == SyntaxKind.Token_Plus)
                left = new SyntaxNode(SyntaxKind.Syntax_Addition, left, _ReadNext(), _ParseMultiplication());
            else if (_currentToken.kind == SyntaxKind.Token_Minus)
                left = new SyntaxNode(SyntaxKind.Syntax_Subtraction, left, _ReadNext(), _ParseMultiplication());
            else break;

        return left;
    }

    private SyntaxNode _ParseMultiplication()
    {
        var left = _ParsePrimary();

        while (true)
            if (_currentToken.kind == SyntaxKind.Token_Star)
                left = new SyntaxNode(SyntaxKind.Syntax_Multiplication, left, _ReadNext(), _ParsePrimary());
            else if (_currentToken.kind == SyntaxKind.Token_Slash)
                left = new SyntaxNode(SyntaxKind.Syntax_Division, left, _ReadNext(), _ParsePrimary());
            else break;

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
