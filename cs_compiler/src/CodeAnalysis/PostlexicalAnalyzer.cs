namespace Nyx.CodeAnalysis;

using Utils;

class PostlexicalAnalyzer: AAnalyzer<SyntaxNode, SyntaxNode>
{
    private SyntaxDefinition _syntax;
    private bool _isNewLine;
    private int _currentIndentDepth;
    private SyntaxNode _currentToken { get => _Peek(0); }
    private TextLocation _currentHiddenLocation { get => new TextLocation(_currentToken.location.pos, 0); }

    public PostlexicalAnalyzer(SyntaxDefinition syntax, List<SyntaxNode> tokens) : base(tokens, tokens.Last())
    {
        _syntax = syntax;
        _currentIndentDepth = 0;
        _isNewLine = true;
    }

    private bool _Discard(SyntaxNode token)
    {
        return
            token.kind == SyntaxKind.Token_Discard ||
            token.kind == SyntaxKind.Token_Comment ||
            _syntax.IsWhiteSpace(token.kind);
    }

    private IEnumerable<SyntaxNode> _GetNext()
    {
        if (_currentToken.kind == SyntaxKind.Token_NewLine)
            _isNewLine = true;

        if (_isNewLine)
        {
            var indentDepth = 0;

            while (_currentToken.kind == SyntaxKind.Token_NewLine) 
                _IncrementPos();
            for (; _currentToken.kind == SyntaxKind.Token_Indent && _currentToken.valid; _IncrementPos())
                    indentDepth++;

            var d = indentDepth - _currentIndentDepth;
            if (d == 0)
                yield return _ReadNext();
            else if (d == 1)
                yield return new SyntaxNode(SyntaxKind.Token_BeginBlock, _currentHiddenLocation);
            else if (d < 0)
                for (var i = 0; i < -d; i++)
                    yield return new SyntaxNode(SyntaxKind.Token_EndBlock, _currentHiddenLocation);
            else
                yield return new SyntaxNode(SyntaxKind.Token_BeginBlock, _currentHiddenLocation, false);
            
            _currentIndentDepth += d;
            _isNewLine = false;
        }
        else
            yield return _ReadNext();
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        while (_currentToken.kind != SyntaxKind.Token_End)
        {
            foreach(var token in _GetNext())
                if (!_Discard(token))
                    yield return token;
        }

        for (var i = 0; i < _currentIndentDepth; i++)
            yield return new SyntaxNode(SyntaxKind.Token_EndBlock, _currentHiddenLocation);

        yield return _currentToken;
    }
}