using System.Collections.Immutable;
using System.Diagnostics;

namespace Nyx.Analysis;

internal static class SyntaxInfo
{
    private static ImmutableDictionary<char, TokenKind> _singleMarker;
    private static ImmutableDictionary<(char, char), TokenKind> _doubleMarker;
    private static ImmutableDictionary<string, TokenKind> _keywords;

    internal const int maxOperatorPrecedence = 7;
    internal const int tokenKindIndex = -10;
    internal const int indentSize = 4;

    internal const char escapeChar = '\\';
    internal const char newLineChar = '\n';
    internal const char endChar = '\0';

    static SyntaxInfo()
    {
        Debug.Assert(TokenKind._error == 0);

        var singleMarker = ImmutableDictionary.CreateBuilder<char, TokenKind>();
        var doubleMarker = ImmutableDictionary.CreateBuilder<(char, char), TokenKind>();
        var keywords = ImmutableDictionary.CreateBuilder<string, TokenKind>();

        // Markers
        singleMarker.Add('\r', TokenKind.discard);
        singleMarker.Add(' ', TokenKind.space);
        singleMarker.Add(newLineChar, TokenKind.newLine);
        singleMarker.Add(endChar, TokenKind.end);

        // charMarker has to be singleMarker or it will break the lexer
        singleMarker.Add('\'', TokenKind.charMarker);
        // stringMarker has to be singleMarker or it will break the lexer
        singleMarker.Add('"', TokenKind.stringMarker);
        singleMarker.Add(escapeChar, TokenKind.escapeChar);
        doubleMarker.Add(('/', '/'), TokenKind.comment);
        doubleMarker.Add(('/', '*'), TokenKind.commentBeginMarker);
        doubleMarker.Add(('*', '/'), TokenKind.commentEndMarker);

        singleMarker.Add('(', TokenKind.lParen);
        singleMarker.Add(')', TokenKind.rParen);
        singleMarker.Add('[', TokenKind.lSquare);
        singleMarker.Add(']', TokenKind.rSquare);

        singleMarker.Add('.', TokenKind.dot);
        singleMarker.Add(',', TokenKind.comma);
        singleMarker.Add(':', TokenKind.colon);
        singleMarker.Add(';', TokenKind.semiColon);
        doubleMarker.Add(('-', '>'), TokenKind.rArrow);
        doubleMarker.Add(('=', '>'), TokenKind.bigRArrow);

        singleMarker.Add('+', TokenKind.plus);
        singleMarker.Add('-', TokenKind.minus);
        singleMarker.Add('*', TokenKind.star);
        singleMarker.Add('/', TokenKind.slash);

        singleMarker.Add('<', TokenKind.less);
        singleMarker.Add('>', TokenKind.greater);
        singleMarker.Add('=', TokenKind.equal);
        singleMarker.Add('%', TokenKind.percent);

        doubleMarker.Add(('=', '='), TokenKind.equalEqual);
        doubleMarker.Add(('!', '='), TokenKind.notEqual);
        doubleMarker.Add(('<', '='), TokenKind.lessEqual);
        doubleMarker.Add(('>', '='), TokenKind.greaterEqual);

        doubleMarker.Add(('+', '+'), TokenKind.plusPlus);
        doubleMarker.Add(('+', '='), TokenKind.plusEqual);
        doubleMarker.Add(('-', '-'), TokenKind.minusMinus);
        doubleMarker.Add(('-', '='), TokenKind.minusEqual);
        doubleMarker.Add(('*', '='), TokenKind.starEqual);
        doubleMarker.Add(('/', '='), TokenKind.slashEqual);
        doubleMarker.Add(('%', '='), TokenKind.percentEqual);

        // Keywords
        keywords.Add("namespace", TokenKind.@namespace);
        keywords.Add("struct", TokenKind.@struct);
        keywords.Add("func", TokenKind.function);
        keywords.Add("var", TokenKind.var);

        keywords.Add("global", TokenKind.global);
        keywords.Add("static", TokenKind.@static);
        
        keywords.Add("mut", TokenKind.mutable);
        keywords.Add("pub", TokenKind.@public);
        keywords.Add("get", TokenKind.get);
        keywords.Add("set", TokenKind.set);

        keywords.Add("return", TokenKind.@return);
        keywords.Add("error", TokenKind.error);

        keywords.Add("if", TokenKind.@if);
        keywords.Add("else", TokenKind.@else);
        keywords.Add("not", TokenKind.not);
        keywords.Add("and", TokenKind.and);
        keywords.Add("or", TokenKind.or);

        keywords.Add("switch", TokenKind.@switch);
        keywords.Add("case", TokenKind.@case);
        keywords.Add("default", TokenKind.@default);
        keywords.Add("for", TokenKind.@for);
        keywords.Add("do", TokenKind.@do);
        keywords.Add("while", TokenKind.@while);
        keywords.Add("continue", TokenKind.@continue);
        keywords.Add("break", TokenKind.@break);

        keywords.Add("include", TokenKind.include);
        keywords.Add("as", TokenKind.@as);

        _singleMarker = singleMarker.ToImmutable();
        _doubleMarker = doubleMarker.ToImmutable();
        _keywords = keywords.ToImmutable();
    }

    internal static TokenKind GetTokenKind(char pattern) => _singleMarker.GetValueOrDefault(pattern);
    internal static TokenKind GetTokenKind((char, char) pattern) => _doubleMarker.GetValueOrDefault(pattern);
    internal static TokenKind GetTokenKind(string name) => _keywords.GetValueOrDefault(name);

    internal static TokenKind GetMarker((char, char) pattern)
    {
        var kind = GetTokenKind(pattern);

        if (kind == TokenKind._error)
            kind = GetTokenKind(pattern.Item2);

        return kind;
    }

    internal static bool IsLineTerminator(TokenKind marker) => marker == TokenKind.newLine || marker == TokenKind.end;
    internal static bool IsLineTerminator((char, char) pattern) => IsLineTerminator(GetMarker(pattern));

    internal static bool IsWhiteSpace(TokenKind marker) => marker == TokenKind.space || marker == TokenKind.newLine || marker == TokenKind.indent;
    internal static bool IsWhiteSpace((char, char) pattern) => IsWhiteSpace(GetMarker(pattern));

    internal static bool IsBlockTerminator(TokenKind marker) => marker == TokenKind.end || marker == TokenKind.endBlock;

    internal static bool IsBlockCommentTerimator(TokenKind marker) => marker == TokenKind.end || marker == TokenKind.commentEndMarker;
    internal static bool IsBlockCommentTerimator((char, char) pattern) => IsBlockCommentTerimator(GetMarker(pattern));
    
    internal static bool IsDiscard(TokenKind marker) => marker == TokenKind.discard || marker == TokenKind.comment || marker == TokenKind.space || marker == TokenKind.indent;

    internal static bool IsEmpty(TokenKind marker) =>  IsDiscard(marker) || marker ==  TokenKind.newLine || marker == TokenKind.beginBlock || marker == TokenKind.endBlock;

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