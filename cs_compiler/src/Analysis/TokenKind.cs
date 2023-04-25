namespace Nyx.Analysis;

internal enum TokenKind
{
    invalidChar = SyntaxInfo.tokenKindIndex,

    @char,
    @string,
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

    charMarker,
    stringMarker,
    escapeChar,
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
