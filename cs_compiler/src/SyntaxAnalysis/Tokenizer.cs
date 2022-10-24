using Utils;

namespace SyntaxAnalysis;

public class TokenizerConfig
{
    public int INDENTSIZE = 4;
}

public class Tokenizer
{
    public string text { get; }
    public TokenizerConfig config { get; }
    
    private int _size;
    private int _pos;

    private char _curC1;
    private char _curC2;
    private int _curStart;
    private int _curLen { get => _pos - _curStart; }
    private TextLocation _curLocation { get => new TextLocation(_curStart, _curLen); }

    private bool _newLine;
    private int _indentLevel;

    public Tokenizer(string text, TokenizerConfig config)
    {
        this.text = text;
        this.config = config;

        _size = text.Length;
        _pos = 0;

        _newLine = true;
    }

    // private char GetNextChar()
    // {
    //     if (pos+1 < 0 || pos+1 >= len)
    //         return '\u0000';
        
    //     return Text[++pos];
    // }

    private char _Peek(int offset=0)
    {
        if (_pos+offset < 0 || _pos+offset >= _size)
            return '\u0000';
        
        return text[_pos+offset];
    }

    public List<SyntaxToken> GetAll()
    {
        for(;;)
        {

        }
    }

    public SyntaxToken GetNext()
    {
        _curStart = _pos;

        _curC1 = _Peek(0);
        _curC2 = _Peek(1);

        if (_newLine)
        {
            _indentLevel = _GetIndentLevel();
            
            if (_indentLevel > 0)
                return new SyntaxToken(SyntaxTokenKind.INDENT, _curLocation);
            else if (_indentLevel < 0)
                ; // TODO: ERROR
        }

    }

    private int _GetIndentLevel()
    {
        // while (char.Equals(_curC1, '\t')) _pos++; // TODO: Replace with spaces

        var indentSize = 0;
        for(; indentSize < config.INDENTSIZE && char.Equals(_curC1, ' '); indentSize++);
        
        if (indentSize % config.INDENTSIZE != 0)
            return indentSize - config.INDENTSIZE;

        return indentSize / config.INDENTSIZE;
    }
}