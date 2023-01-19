namespace Nyx.Analysis;

using Diagnostics;
using Utils;

public class LexicalAnalyzer : Analyzer<char, SyntaxNode>
{
    SyntaxDefinition _syntax;
    string _text;
    int _start;
    char _currentChar { get => _Peek(0); }
    char _nextChar { get => _Peek(1); }
    int _length { get => _pos - _start; }
    TextLocation _location { get => new TextLocation(_start, _length); }
    string _subString { get => _text.Substring(_start, _length); }
    bool _newLine;


    public LexicalAnalyzer(SyntaxDefinition syntax, string text) : base(text.ToList(), syntax.endSymbol)
    {
        _syntax = syntax;
        _text = text;
        _newLine = true;
    }

    public SyntaxNode GetNext()
    {
        _start = _pos;

        // lex indents after a new line
        if (_newLine)
        {
            _SkipBlankLines();

            while (_length < _syntax.indentSize && _syntax.GetSingleTokenKind(_currentChar) == SyntaxKind.Token_Space) 
                _pos++;

            if (_length % _syntax.indentSize != 0)
            {
                diagnostics.Add(new InvalidIndent(_location));
                return new SyntaxNode(SyntaxKind.Token_Indent, _location, isValid: false);
            }
            else if (_length > 0)
                return new SyntaxNode(SyntaxKind.Token_Indent, _location);

            _newLine = false;
        }

        // lex numbers
        if (char.IsDigit(_currentChar))
        {
            while (char.IsDigit(_currentChar)) 
                _pos++;
            // TODO: Handle '.'
            return new SyntaxNode(SyntaxKind.Token_Number, _location, value: _subString);
        }
        // lex strings
        else if (_syntax.GetSingleTokenKind(_currentChar) == SyntaxKind.Token_StringMarker)
        {
            var terminator = _currentChar;

            while (_currentChar == _syntax.escapeSymbol || _nextChar != terminator)
            {
                _pos++;

                if (_syntax.IsLineTerminator(_currentChar))
                {
                    diagnostics.Add(new StringNotTerminated(_location, terminator));
                    return new SyntaxNode(SyntaxKind.Token_String, _location, isValid: false);
                }
            }
            _pos += 2;

            return new SyntaxNode(SyntaxKind.Token_String, _location);
        }
        else if (_syntax.GetDoubleTokenKind((_currentChar, _nextChar)) == SyntaxKind.Token_CommentMarker)
        {
            while (!_syntax.IsLineTerminator(_currentChar))
                _IncrementPos();
            
            return new SyntaxNode(SyntaxKind.Token_Comment, _location);
        }
        // lex operators and names
        else
        {
            var kind = _syntax.GetDoubleTokenKind((_currentChar, _nextChar));

            if (kind == SyntaxKind.Token_Error)
            {
                kind = _syntax.GetSingleTokenKind(_currentChar);

                if (kind == SyntaxKind.Token_Error)
                {
                    while (char.IsLetterOrDigit(_currentChar) || _currentChar == '_') 
                        _pos++;

                    if (_length > 0)
                    {
                        kind = _syntax.GetKeyword(_text.Substring(_start, _length));

                        if (kind != SyntaxKind.Token_Error)
                            return new SyntaxNode(kind, _location);

                        return new SyntaxNode(SyntaxKind.Token_Literal, _location, value: _subString);
                    }

                    _pos++;
                    return new SyntaxNode(SyntaxKind.Token_InvalidChar, _location, value: _subString);
                }
                else if (kind == SyntaxKind.Token_NewLine)
                    _newLine = true;
            }
            else
                _pos++;
            _pos++;

            return new SyntaxNode(kind, _location);
        }
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        SyntaxNode token;

        do
        {
            token = GetNext();
            yield return token;
        }
        while (!isFinished);

        yield return GetNext();
    }

    void _SkipBlankLines()
    {
        int offset = 0;
        int blankLineCount = 0;

        while (true)
        {
            int currentOffset = 0;

            while (char.IsWhiteSpace(_Peek(offset + currentOffset)))
                currentOffset++;

            if (_Peek(offset + currentOffset) != _syntax.newLineSymbol)
                break;
            // TODO: checks special case with endSymbol

            offset += currentOffset;
            blankLineCount++;
        }

        _pos += offset;
    }
}