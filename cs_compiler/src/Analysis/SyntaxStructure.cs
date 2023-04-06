using System.Collections;
using System.Diagnostics;
using System.Text;
using Nyx.Utils;

namespace Nyx.Analysis;

internal enum TokenKind
{
    invalidChar = _SyntaxInfo.tokenKindIndex,

    @string,
    @char,
    identifier,
    number,
    comment,

    indent,
    beginBlock,
    endBlock,
    
    // default
    _error,

    // Markers
    discard,
    space,
    newLine,
    end,

    stringMarker,
    escapeChar,
    charMarker,
    commentMarker,
    commentBeginMarker,
    commentEndMarker,

    lParen,
    rParen,
    lSquare,
    rSquare,

    dot,
    comma,
    colon,
    semiColon,
    rArrow,
    bigRArrow,

    plus,
    minus,
    star,
    slash,

    less,
    greater,
    equal,
    percent,

    equalEqual,
    notEqual,
    lessEqual,
    greaterEqual,

    plusPlus,
    plusEqual,
    minusMinus,
    minusEqual,
    starEqual,
    slashEqual,
    percentEqual,

    // Keywords
    @namespace,
    @struct,
    function,
    var,

    global,
    @static,
    
    mutable,
    @public,
    get,
    set,

    @return,
    error,

    @if,
    @else,
    not,
    and,
    or,

    @switch,
    @case,
    @default,
    @for,
    @do,
    @while,
    @continue,
    @break,

    include,
    @as,
}

internal static class _SyntaxInfo
{
    private static Dictionary<char, TokenKind> _singleMarker = new Dictionary<char, TokenKind>();
    private static Dictionary<(char, char), TokenKind> _doubleMarker = new Dictionary<(char, char), TokenKind>();
    private static Dictionary<string, TokenKind> _keywords = new Dictionary<string, TokenKind>();

    internal const int maxOperatorPrecedence = 7;
    internal const int tokenKindIndex = -9;
    internal static int indentSize = 4;

    static char _escapeChar = '\\';
    internal static char escapeChar
    {
        get => _escapeChar;
        set { _escapeChar = value; DefineToken(value, TokenKind.escapeChar); }
    }

    static char _newLine = '\n';
    internal static char newLine
    {
        get => _newLine;
        set { _newLine = value; DefineToken(value, TokenKind.newLine); }
    }

    static char _end = '\0';
    internal static char end
    {
        get => _end;
        set { _end = value; DefineToken(value, TokenKind.end); }
    }

    static _SyntaxInfo()
    {
        // Markers
        DefineToken('\r', TokenKind.discard);
        DefineToken(' ', TokenKind.space);
        DefineToken(_newLine, TokenKind.newLine);
        DefineToken(_end, TokenKind.end);

        DefineToken('"', TokenKind.@stringMarker);
        DefineToken(_escapeChar, TokenKind.escapeChar);
        DefineToken('\'', TokenKind.@char);
        DefineToken(('/', '/'), TokenKind.comment);
        DefineToken(('/', '*'), TokenKind.commentBeginMarker);
        DefineToken(('*', '/'), TokenKind.commentEndMarker);

        DefineToken('(', TokenKind.lParen);
        DefineToken(')', TokenKind.rParen);
        DefineToken('[', TokenKind.lSquare);
        DefineToken(']', TokenKind.rSquare);

        DefineToken('.', TokenKind.dot);
        DefineToken(',', TokenKind.comma);
        DefineToken(':', TokenKind.colon);
        DefineToken(';', TokenKind.semiColon);
        DefineToken(('-', '>'), TokenKind.rArrow);
        DefineToken(('=', '>'), TokenKind.bigRArrow);

        DefineToken('+', TokenKind.plus);
        DefineToken('-', TokenKind.minus);
        DefineToken('*', TokenKind.star);
        DefineToken('/', TokenKind.slash);

        DefineToken('<', TokenKind.less);
        DefineToken('>', TokenKind.greater);
        DefineToken('=', TokenKind.equal);
        DefineToken('%', TokenKind.percent);

        DefineToken(('=', '='), TokenKind.equalEqual);
        DefineToken(('!', '='), TokenKind.notEqual);
        DefineToken(('<', '='), TokenKind.lessEqual);
        DefineToken(('>', '='), TokenKind.greaterEqual);

        DefineToken(('+', '+'), TokenKind.plusPlus);
        DefineToken(('+', '='), TokenKind.plusEqual);
        DefineToken(('-', '-'), TokenKind.minusMinus);
        DefineToken(('-', '='), TokenKind.minusEqual);
        DefineToken(('*', '='), TokenKind.starEqual);
        DefineToken(('/', '='), TokenKind.slashEqual);
        DefineToken(('%', '='), TokenKind.percentEqual);

        // Keywords
        DefineToken("namespace", TokenKind.@namespace);
        DefineToken("struct", TokenKind.@struct);
        DefineToken("func", TokenKind.function);
        DefineToken("var", TokenKind.var);

        DefineToken("global", TokenKind.global);
        DefineToken("static", TokenKind.@static);
        
        DefineToken("mut", TokenKind.mutable);
        DefineToken("pub", TokenKind.@public);
        DefineToken("get", TokenKind.get);
        DefineToken("set", TokenKind.set);

        DefineToken("return", TokenKind.@return);
        DefineToken("error", TokenKind.error);

        DefineToken("if", TokenKind.@if);
        DefineToken("else", TokenKind.@else);
        DefineToken("not", TokenKind.not);
        DefineToken("and", TokenKind.and);
        DefineToken("or", TokenKind.or);

        DefineToken("switch", TokenKind.@switch);
        DefineToken("case", TokenKind.@case);
        DefineToken("default", TokenKind.@default);
        DefineToken("for", TokenKind.@for);
        DefineToken("do", TokenKind.@do);
        DefineToken("while", TokenKind.@while);
        DefineToken("continue", TokenKind.@continue);
        DefineToken("break", TokenKind.@break);

        DefineToken("include", TokenKind.include);
        DefineToken("as", TokenKind.@as);
    }

    internal static void DefineToken(char pattern, TokenKind kind) 
    {
        Debug.Assert(kind > 0);
        _singleMarker.Add(pattern, kind);
    }

    internal static void DefineToken((char, char) pattern, TokenKind kind) 
    {
        Debug.Assert(kind > 0);
        _doubleMarker.Add(pattern, kind);
    }

    internal static void DefineToken(string name, TokenKind kind)
    {
        Debug.Assert(kind > 0);
        _keywords.Add(name, kind);
    }

    internal static TokenKind GetTokenKind(char pattern) => _singleMarker.GetValueOrDefault(pattern);
    internal static TokenKind GetTokenKind((char, char) pattern) => _doubleMarker.GetValueOrDefault(pattern);
    internal static TokenKind GetTokenKind(string name) => _keywords.GetValueOrDefault(name);

    internal static bool IsLineTerminator(TokenKind marker) => marker == TokenKind.newLine || marker == TokenKind.end;
    internal static bool IsLineTerminator(char pattern) => IsLineTerminator(GetTokenKind(pattern));

    internal static bool IsWhiteSpace(TokenKind marker) => marker == TokenKind.space || marker == TokenKind.newLine || marker == TokenKind.indent;

    internal static bool IsBlockTerminator(TokenKind marker) => marker == TokenKind.end || marker == TokenKind.endBlock;

    internal static int GetOperatorPrecedence(TokenKind marker)
    {
        switch (marker)
        {
            case TokenKind.equal:
            case TokenKind.plusEqual:
            case TokenKind.minusEqual:
            case TokenKind.starEqual:
            case TokenKind.slashEqual:
            case TokenKind.percentEqual:
                return 7;
            case TokenKind.or:
                return 6;
            case TokenKind.and:
                return 5;
            case TokenKind.equalEqual:
            case TokenKind.notEqual:
                return 4;
            case TokenKind.greater:
            case TokenKind.greaterEqual:
            case TokenKind.less:
            case TokenKind.lessEqual:
                return 3;
            case TokenKind.plus:
            case TokenKind.minus:
                return 2;
            case TokenKind.star:
            case TokenKind.slash:
                return 1;
            default: 
                return 0;
        }
    }
}

internal class Location
{
    internal string file { get; }
    internal int index { get; }
    internal int length { get; }
    internal int end { get => index + end; }
    internal int line { get; }
    internal int lineEnd { get; }

    internal Location(string file, int index, int length, int line, int lineEnd)
    {
        this.file = file;
        this.index = index;
        this.length = length;
        this.line = line;
        this.lineEnd = lineEnd;
    }
}

internal abstract class _Node { }

internal class Token : _Node
{ 
    internal TokenKind kind { get; }
    internal TextLocation location { get; }

    internal Token(TokenKind kind, TextLocation location)
    {
        this.kind = kind;
        this.location = location;
    }
}

internal abstract class Source
{
    internal abstract string name { get; }

    internal abstract TextReader TextReader();
}

internal class SourceString : Source
{
    internal override string name => "<string>";
    string _source;

    internal SourceString(string source)
    {
        _source = source;
    }

    internal override TextReader TextReader() => new StringReader(_source);
}

internal class SourceFile : Source
{
    internal override string name { get; }

    internal SourceFile(string path)
    {
        name = path;
    }

    internal override TextReader TextReader() => File.OpenText(name);
}

internal class _Lexer : IEnumerator<Token>
{
    Token IEnumerator<Token>.Current => throw new NotImplementedException();
    public object Current => throw new NotImplementedException();

    TextReader _source;

    bool _finished = false;

    char _last = _SyntaxInfo.end;
    char _current = _SyntaxInfo.end;
    char _next = _SyntaxInfo.end;

    char _Next()
    {
        _last = _current;
        _current = _next;
        _next = _Read();

        return _last;
    }

    char _Read()
    {
        var result = _source.Read();

        if (result < 0)
            return _SyntaxInfo.end;

        return (char) result; 
    }

    public bool MoveNext()
    {
        _Next();
        return _current == _SyntaxInfo.end && _next == _SyntaxInfo.end;
    }

    public void Reset()
    {
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}