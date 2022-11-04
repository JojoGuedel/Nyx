using Utils;

namespace SyntaxAnalysis;

public class Tokenizer
{
    public string text { get; }
    public SyntaxDefinition syntax { get; }

    private int _size;
    private int _pos;

    private char _curC1 { get => _Peek(0); }
    private char _curC2 { get => _Peek(1); }
    private int _curStart;
    private int _curLen { get => _pos - _curStart; }
    private TextLocation _curLocation { get => new TextLocation(_curStart, _curLen); }

    private bool _newLine;

    public Tokenizer(string text, SyntaxDefinition syntax)
    {
        this.text = text;
        this.syntax = syntax;

        _size = text.Length;
        _pos = 0;

        _newLine = true;
    }

    private char _Peek(int offset = 0)
    {
        if (_pos + offset < 0 || _pos + offset >= _size)
            return syntax.endSymbol;

        return text[_pos + offset];
    }

    public IEnumerable<SyntaxNode> GetTokens()
    {
        SyntaxNode token;

        do
        {
            token = GetNextToken();

            if (token.kind != SyntaxKind._Discard)
                yield return token;
        }
        while (token.kind != SyntaxKind.Token_End);
    }

    public SyntaxNode GetNextToken()
    {
        _curStart = _pos;

        // tokenize indents after a new line
        if (_newLine)
        {
            _SkipBlankLines();

            for (; _curLen < syntax.indentSize && char.Equals(_curC1, syntax.indentSymbol); _pos++) ;

            if (_curLen % syntax.indentSize != 0)
                return new SyntaxNode(SyntaxKind._Indent, _curLocation, false);
            else if (_curLen > 0)
                return new SyntaxNode(SyntaxKind._Indent, _curLocation);

            _newLine = false;
        }

        // tokenize numbers
        if (char.IsDigit(_curC1))
        {
            while (char.IsDigit(_curC1)) _pos++;
            // TODO: Handle '_' and '.'
            return new SyntaxNode(SyntaxKind.Token_Number, _curLocation);
        }
        // tokenize strings
        else if (syntax.GetSingleTokenKind(_curC1) == SyntaxKind._StringMarker)
        {
            var terminator = _curC1;

            while (char.Equals(_curC1, '\\') || !char.Equals(_curC2, terminator))
            {
                _pos++;

                if (syntax.IsLineTerminator(_curC1))
                    // TODO: add this to diagnostics
                    return new SyntaxNode(SyntaxKind.Token_String, _curLocation, false);
            }
            _pos += 2;

            return new SyntaxNode(SyntaxKind.Token_String, _curLocation);
        }
        // tokenize operators and names
        else
        {
            var kind = syntax.GetDoubleTokenKind((_curC1, _curC2));

            if (kind == SyntaxKind._Error)
            {
                kind = syntax.GetSingleTokenKind(_curC1);

                if (kind == SyntaxKind._Error)
                {
                    // TODO: filter unused names ('_')
                    while (char.IsLetterOrDigit(_curC1) || char.Equals(_curC1, '_')) _pos++;

                    if (_curLen > 0)
                    {
                        kind = syntax.GetKeyword(text.Substring(_curStart, _curLen));

                        if (kind != SyntaxKind._Error)
                            return new SyntaxNode(kind, _curLocation);

                        return new SyntaxNode(SyntaxKind.Token_Identifier, _curLocation);
                    }

                    _pos += 1;
                    return new SyntaxNode(SyntaxKind.Token_InvalidChar, _curLocation);
                }
                else if (kind == SyntaxKind.Token_NewLine)
                    _newLine = true;
            }
            else
                _pos++;
            _pos++;

            return new SyntaxNode(kind, _curLocation);
        }
    }

    private void _SkipBlankLines()
    {
        int offset = 0;
        int blankLineCount = 0;

        while (true)
        {
            int curOffset = 0;

            while (char.IsWhiteSpace(_Peek(offset + curOffset))) curOffset++;

            if (!char.Equals(_Peek(offset + curOffset), syntax.newLineSymbol))
                break;
            // TODO: checks special case with endSymbol

            offset += curOffset;
            blankLineCount++;
        }

        _pos += offset;
    }
}