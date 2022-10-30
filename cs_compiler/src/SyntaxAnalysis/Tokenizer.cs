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

    private char _Peek(int offset=0)
    {
        if (_pos+offset < 0 || _pos+offset >= _size)
            return '\u0000';
        
        return text[_pos+offset];
    }

    public SyntaxToken GetNextToken()
    {
        _curStart = _pos;

        if (_newLine)
        {
            for(; _curLen < syntax.indentSize && char.Equals(_curC1, syntax.indentSymbol); _pos++);
            
            if (_curLen % syntax.indentSize != 0)
                return new SyntaxToken(SyntaxTokenKind.ERROR, _curLocation);
            else if (_curLen > 0)
                return new SyntaxToken(SyntaxTokenKind.INDENT, _curLocation);
            
            _newLine = false;
        }

        if (char.IsDigit(_curC1))
        {
            while (char.IsDigit(_curC1)) _pos++;
            // TODO: Handle '_' and '.'
            return new SyntaxToken(SyntaxTokenKind.NUMBER, _curLocation);
        }
        if (syntax.GetSingleTokenKind(_curC1) == SyntaxTokenKind.STRING)
        {
            var terminator = _curC1;

            while(char.Equals(_curC1, '\\') || !char.Equals(_curC2, terminator))
            {
                _pos++;

                if (char.Equals(_curC1, syntax.endSymbol) || char.Equals(_curC1, syntax.newLineSymbol))
                    return new SyntaxToken(SyntaxTokenKind.ERROR, _curLocation);
            }
            _pos += 2;

            return new SyntaxToken(SyntaxTokenKind.STRING, _curLocation);
        }
        else
        {
            var kind = syntax.GetDoubleTokenKind((_curC1, _curC2));

            if (kind == SyntaxTokenKind.ERROR)
                kind = syntax.GetSingleTokenKind(_curC1);
            else
                _pos++;
            _pos++;
            
            return new SyntaxToken(kind, _curLocation);
        }
    }
}