using Nyx.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class PostlexicalAnalyzer : Analyzer<SyntaxNode, SyntaxNode>
{
    public DiagnosticCollection diagnostics;
    private SyntaxDefinition _syntax;
    private int _currentIndentDepth;
    private int _currentLineIndentDepth;
    private SyntaxNode _currentToken { get => _Peek(0); }
    private TextLocation _currentHiddenLocation { get => new TextLocation(_currentToken.location.pos, 0); }

    public PostlexicalAnalyzer(SyntaxDefinition syntax, List<SyntaxNode> tokens) : base(tokens, tokens.Last())
    {
        diagnostics = new DiagnosticCollection();
        _syntax = syntax;
        _currentIndentDepth = 0;
        _currentLineIndentDepth = 0;
    }

    private bool _Discard(SyntaxNode token)
    {
        return
            token.kind == SyntaxKind.Token_Discard ||
            token.kind == SyntaxKind.Token_Comment ||
            _syntax.IsWhiteSpace(token.kind);
    }

    private bool _IsEmptyToken(SyntaxNode token)
    {
        return
            _Discard(token) ||
            token.kind == SyntaxKind.Token_BeginBlock ||
            token.kind == SyntaxKind.Token_EndBlock;
    }

    private bool _IsEmptyLine(List<SyntaxNode> line)
    {
        var isEmpty = true;

        foreach(var token in line)
        {
            if (!_IsEmptyToken(token))
            {
                isEmpty = false;
                break;
            }
        }

        return isEmpty;
    }

    List<SyntaxNode> GetNextLine()
    {
        var line = new List<SyntaxNode>();
        var indentDepth = 0;

        while (_currentToken.kind == SyntaxKind.Token_Indent && _currentToken.valid)
        {
            _IncrementPos();
            indentDepth++;
        }

        var d = indentDepth - _currentIndentDepth;

        if (d == 1)
            line.Add(new SyntaxNode(SyntaxKind.Token_BeginBlock, _currentHiddenLocation));
        else if (d < 0)
            for (var i = 0; i < -d; i++)
                line.Add(new SyntaxNode(SyntaxKind.Token_EndBlock, _currentHiddenLocation));
        else if (d > 1)
            diagnostics.Add(new TooManyIndents(_currentToken.location));
        
        _currentLineIndentDepth = indentDepth;

        while(!_syntax.IsLineTerminator(_currentToken.kind))
            line.Add(_ReadNext());

        if (_currentToken.kind == SyntaxKind.Token_End)
            for (var i = 0; i < _currentIndentDepth + d; i++)
                line.Add(new SyntaxNode(SyntaxKind.Token_EndBlock, _currentHiddenLocation));
        line.Add(_ReadNext());
        
        return line;
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        while (!isFinished)
        {
            var currentLine = GetNextLine();

            if (!_IsEmptyLine(currentLine))
            {
                foreach(var token in currentLine)
                    if (!_Discard(token))
                        yield return token;

                _currentIndentDepth = _currentLineIndentDepth;
            }
        }
    }
}