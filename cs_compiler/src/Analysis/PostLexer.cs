using Nyx.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class PostLexer : Analyzer<LexerNode, LexerNode>
{
    SyntaxInfo _syntax;
    int _currentIndentDepth;
    int _currentLineIndentDepth;
    LexerNode _currentToken { get => _Peek(0); }
    TextLocation _currentHiddenLocation { get => new TextLocation(_currentToken.location.pos, 0); }

    public PostLexer(SyntaxInfo syntax, List<LexerNode> tokens) : base(tokens, tokens.Last())
    {
        _syntax = syntax;
        _currentIndentDepth = 0;
        _currentLineIndentDepth = 0;
    }

    bool _Discard(LexerNode token)
    {
        return
            token.kind == SyntaxKind.Token_Discard ||
            token.kind == SyntaxKind.Token_Comment ||
            _syntax.IsWhiteSpace(token.kind);
    }

    bool _IsEmptyToken(LexerNode token)
    {
        return
            _Discard(token) ||
            token.kind == SyntaxKind.Token_BeginBlock ||
            token.kind == SyntaxKind.Token_EndBlock;
    }

    bool _IsEmptyLine(List<LexerNode> line)
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

    List<LexerNode> GetNextLine()
    {
        var line = new List<LexerNode>();
        var indentDepth = 0;

        while (_currentToken.kind == SyntaxKind.Token_Indent)
        {
            _IncrementPos();
            indentDepth++;
        }

        var d = indentDepth - _currentIndentDepth;

        if (d == 1)
            line.Add(new LexerNode(SyntaxKind.Token_BeginBlock, _currentHiddenLocation));
        else if (d < 0)
            for (var i = 0; i < -d; i++)
                line.Add(new LexerNode(SyntaxKind.Token_EndBlock, _currentHiddenLocation));
        else if (d > 1)
            diagnostics.Add(new TooManyIndents(_currentToken.location));
        
        _currentLineIndentDepth = indentDepth;

        while(!_syntax.IsLineTerminator(_currentToken.kind))
            line.Add(_ReadNext());

        if (_currentToken.kind == SyntaxKind.Token_End)
            for (var i = 0; i < _currentIndentDepth + d; i++)
                line.Add(new LexerNode(SyntaxKind.Token_EndBlock, _currentHiddenLocation));
        line.Add(_ReadNext());
        
        return line;
    }

    public override IEnumerable<LexerNode> GetAll()
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