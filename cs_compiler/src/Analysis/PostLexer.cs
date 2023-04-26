using System.Diagnostics;

namespace Nyx.Analysis;

internal class PostLexer
{
    IEnumerator<Token> _source;

    bool _finished = false;

    Token _last = Token.Empty();
    Token _current = Token.Empty();

    int _indent = 0;
    int _lineIndent = 0;

    public PostLexer(IEnumerator<Token> source)
    {
        _source = source;
        
        _Next();
    }

    Token _Next()
    {
        _last = _current;

        if (!_source.MoveNext())
        {
            _current = Token.Empty();
            _finished = true;
        }
        else
            _current = _source.Current;

        return _last;
    }

    List<Token> _GetLine()
    {
        var line = new List<Token>();
        var indent = 0;

        while (_current.kind == TokenKind.indent)
        {
            _Next();
            indent++;
        }

        var d = indent - _indent;
        if (d == 1)
            line.Add(new Token(TokenKind.beginBlock, _current.location.Point()));
        else if (d < 0)
            for (var i = 0; i < -d; i++)
                line.Add(new Token(TokenKind.endBlock, _current.location.Point()));
        else if (d > 1)
            // TODO: diagnostics
            throw new NotImplementedException();
        
        _lineIndent = indent;

        while(!SyntaxInfo.IsLineTerminator(_current.kind))
            line.Add(_Next());

        if (_current.kind == TokenKind.end)
            for (var i = 0; i < _indent; i++)
                line.Add(new Token(TokenKind.endBlock, _current.location.Point()));

        line.Add(_Next());
        
        return line;
    }

    bool _IsEmptyLine(IEnumerable<Token> line)
    {
        foreach(var token in line)
            if (!SyntaxInfo.IsEmpty(token.kind))
                return false;

        return true;
    }

    internal IEnumerable<Token> Analyze()
    {
        while (!_finished)
        {
            var line = _GetLine();

            if (_IsEmptyLine(line))
                continue;

            foreach(var token in line)
                if (!SyntaxInfo.IsDiscard(token.kind))
                    yield return token;

            _indent = _lineIndent;
        }
    }
}