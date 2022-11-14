using Diagnostics;
using SyntaxAnalysis;

public class Parser
{
    public DiagnosticCollection diagnostics { get; }

    private readonly List<SyntaxNode> _tokens;
    private SyntaxDefinition _syntax;
    private int _pos;

    private SyntaxNode _curTok { get => _Peek(0); }

    public Parser(List<SyntaxNode> tokens, SyntaxDefinition syntax)
    {
        diagnostics = new DiagnosticCollection();

        _tokens = tokens;
        _syntax = syntax;
        _pos = 0;
    }

    private void _IncrementPos()
    {
        if (_pos < _tokens.Count) _pos++;
    }

    private SyntaxNode _Peek(int offset)
    {
        if (_pos + offset >= 0 && _pos + offset < _tokens.Count)
            return _tokens[_pos + offset];

        return _tokens.Last();
    }

    private SyntaxNode _NextToken()
    {
        var token = _curTok;
        _IncrementPos();
        return token;
    }

    private SyntaxNode _MatchToken(params SyntaxKind[] kinds)
    {
        foreach (var kind in kinds)
            if (_curTok.kind == kind) return _NextToken();

        var ret = new SyntaxNode(kinds[0], _curTok.location);
        diagnostics.Add(new Error_UnexpectedToken(_curTok, kinds));
        _IncrementPos();
        return ret;
    }

    private void _SkipWhiteSpaces()
    {
        while (_syntax.IsWhiteSpace(_curTok.kind)) _pos++;
    }

    public SyntaxNode Parse()
    {
        return _ParseCompilationUnit();   
    }

    private SyntaxNode _ParseCompilationUnit()
    {
        List<SyntaxNode> nodes = new List<SyntaxNode>();

        while (_curTok.kind != SyntaxKind.Token_End)
            nodes.Add(_ParseDeclarationStatement());
        nodes.Add(_curTok);

        return new SyntaxNode(SyntaxKind.Syntax_CompilationUnit, nodes);
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
        if (_curTok.kind == SyntaxKind.Token_Colon)
            return _ParseTypeClause();

        return SyntaxNode.EmptySyntax(_curTok.location.pos + _curTok.location.len);
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
            if (_curTok.kind == SyntaxKind.Token_Plus)
                left = new SyntaxNode(SyntaxKind.Syntax_Addition, left, _NextToken(), _ParseMultiplication());
            else if (_curTok.kind == SyntaxKind.Token_Minus)
                left = new SyntaxNode(SyntaxKind.Syntax_Subtraction, left, _NextToken(), _ParseMultiplication());
            else break;

        return left;
    }

    private SyntaxNode _ParseMultiplication()
    {
        var left = _ParsePrimary();

        while (true)
            if (_curTok.kind == SyntaxKind.Token_Star)
                left = new SyntaxNode(SyntaxKind.Syntax_Multiplication, left, _NextToken(), _ParsePrimary());
            else if (_curTok.kind == SyntaxKind.Token_Slash)
                left = new SyntaxNode(SyntaxKind.Syntax_Division, left, _NextToken(), _ParsePrimary());
            else break;

        return left;
    }

    private SyntaxNode _ParsePrimary()
    {
        switch (_curTok.kind)
        {
            case SyntaxKind.Token_Identifier:
            case SyntaxKind.Token_Number:
            case SyntaxKind.Token_String:
                return _NextToken();

            default:
                return new SyntaxNode(SyntaxKind.Syntax_Error, _curTok.location);
        }
    }
}