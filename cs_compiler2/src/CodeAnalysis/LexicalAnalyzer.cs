namespace CodeAnalysis;

using Diagnostics;
using Utils;

class LexicalAnalyzer : AAnalyzer<char, SyntaxNode>
{
    public DiagnosticCollection diagnostics;
    SyntaxDefinition _syntax;
    string _text;
    int _start;
    char _currentChar { get => _Peek(0); }
    char _nextChar { get => _Peek(1); }
    int _length { get => _pos - _start; }
    TextLocation _location { get => new TextLocation(_start, _length); }
    bool _newLine;


    public LexicalAnalyzer(SyntaxDefinition syntax, string text) : base(text.ToList(), syntax.endSymbol)
    {
        diagnostics = new DiagnosticCollection();
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

            while (_length < _syntax.indentSize && _syntax.GetSingleTokenKind(_currentChar).Equals(SyntaxKind.Token_Space)) 
                _pos++;

            if (_length % _syntax.indentSize != 0)
                return new SyntaxNode(SyntaxKind.Token_Indent, _location, false);
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
            return new SyntaxNode(SyntaxKind.Token_Number, _location);
        }
        // lex strings
        else if (_syntax.GetSingleTokenKind(_currentChar) == SyntaxKind.Token_StringMarker)
        {
            var terminator = _currentChar;

            while (char.Equals(_currentChar, _syntax.escapeSymbol) || !char.Equals(_nextChar, terminator))
            {
                _pos++;

                if (_syntax.IsLineTerminator(_currentChar))
                {
                    diagnostics.Add(new Error_StringNotClosed(_location, terminator));
                    return new SyntaxNode(SyntaxKind.Token_String, _location, false);
                }
            }
            _pos += 2;

            return new SyntaxNode(SyntaxKind.Token_String, _location);
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
                    while (char.IsLetterOrDigit(_currentChar) || char.Equals(_currentChar, '_')) 
                        _pos++;

                    if (_length > 0)
                    {
                        kind = _syntax.GetKeyword(_text.Substring(_start, _length));

                        if (kind != SyntaxKind.Token_Error)
                            return new SyntaxNode(kind, _location);

                        return new SyntaxNode(SyntaxKind.Token_Identifier, _location);
                    }

                    _pos++;
                    return new SyntaxNode(SyntaxKind.Token_InvalidChar, _location);
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
        while (token.kind != SyntaxKind.Token_End);
    }

    private void _SkipBlankLines()
    {
        int offset = 0;
        int blankLineCount = 0;

        while (true)
        {
            int currentOffset = 0;

            while (char.IsWhiteSpace(_Peek(offset + currentOffset)))
                currentOffset++;

            if (!char.Equals(_Peek(offset + currentOffset), _syntax.newLineSymbol))
                break;
            // TODO: checks special case with endSymbol

            offset += currentOffset;
            blankLineCount++;
        }

        _pos += offset;
    }
}