using Utils;

namespace SyntaxAnalysis;

public class IndentAnalyzer
{
    private readonly List<SyntaxNode> _tokens;
    private int _pos;

    private bool _newLine;
    private int _curIndentDepth;
    private SyntaxNode _curTok { get => _Peek(0); }
    private TextLocation _curHiddenLocation { get => new TextLocation(_curTok.location.pos, 0); }

    public IndentAnalyzer(List<SyntaxNode> tokens)
    {
        _tokens = tokens;
        _pos = 0;

        _curIndentDepth = 0;
        _newLine = true;
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

    private IEnumerable<SyntaxNode> _GetNext()
    {
        if (_curTok.kind == SyntaxKind.Token_NewLine)
            _newLine = true;

        if (_newLine)
        {
            var indentDepth = 0;

            while (_curTok.kind == SyntaxKind.Token_NewLine) _IncrementPos();
            for (; _curTok.kind == SyntaxKind.Token_Indent && _curTok.valid; _IncrementPos())
                    indentDepth++;

            var d = indentDepth - _curIndentDepth;
            if (d == 0)
                yield return _NextToken();
            else if (d == 1)
                yield return new SyntaxNode(SyntaxKind.Token_BeginBlock, _curHiddenLocation);
            else if (d < 0)
                for (var i = 0; i < -d; i++)
                    yield return new SyntaxNode(SyntaxKind.Token_EndBlock, _curHiddenLocation);
            else
                yield return new SyntaxNode(SyntaxKind.Token_BeginBlock, _curHiddenLocation, false);
            
            _curIndentDepth += d;
            _newLine = false;
        }
        else
            yield return _NextToken();
    }

    public IEnumerable<SyntaxNode> GetAll()
    {
        while (_curTok.kind != SyntaxKind.Token_End)
        {
            foreach(var nextTok in _GetNext())
                yield return nextTok;
        }

        for (var i = 0; i < _curIndentDepth; i++)
            yield return new SyntaxNode(SyntaxKind.Token_EndBlock, _curHiddenLocation);

        yield return _curTok;
    }
}