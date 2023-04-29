using System.Collections.Immutable;
using System.Diagnostics;

namespace Nyx.Analysis;

internal class Parser
{
    IEnumerator<Token> _source;

    Token _last = Token.Empty();
    Token _current = Token.Empty();

    internal Parser(IEnumerator<Token> source)
    {
        _source = source;

        _Next();
    }

    Token _Next()
    {
        Debug.Assert(_source.MoveNext());

        _last = _current;
        _current = _source.Current;

        return _last;
    }

    Token _Match(TokenKind kind)
    {
        if (_current.kind == kind)
            return _Next();
        
        // throw new NotImplementedException();
        // TODO: adjust diagnostics to new node type
        // diagnostics.Add(new UnexpectedToken(_current, kind));
        return new ErrorToken(_Next());
    }

    ImmutableArray<T> _ParseUntil<T>(TokenKind terminator, Func<T> parseCallBack, TokenKind separator = TokenKind.comma)
    {
        var nodes = new List<T>();
        
        while (_current.kind != terminator)
        {
            nodes.Add(parseCallBack());

            if (_current.kind != separator)
                break;

            _Next();
        }

        return nodes.ToImmutableArray();
    }

    Identifier _ParseIdentifier() => new Identifier((ValueToken)_Match(TokenKind.identifier));

    Expression _ParseName()
    {
        Expression expr = _ParseIdentifier();

        while (_current.kind == TokenKind.dot)
            expr = _ParseMemberAccess(expr);

        return expr;
    }

    CompilationUnit _ParseCompilationUnit()
    {
        var members = new List<Member>();

        while (_current.kind != TokenKind.end)
            members.Add(_ParseMember());
        
        return new CompilationUnit(members.ToImmutableArray(), _current);
    }

    Member _ParseMember()
    {
        switch(_current.kind)
        {
            case TokenKind.function:
                return _ParseFunction();
            case TokenKind.@struct:
                throw new NotImplementedException();
            case TokenKind.global:
                return _ParseGlobal();
            case TokenKind.newLine:
                _Next();
                return _ParseMember();
            default: 
                // TODO: add diagnostics
                throw new NotImplementedException();
        }
    }

    Function _ParseFunction() => new Function(
        _ParseModifiers(),
        _Match(TokenKind.function),
        _ParseName(),
        _Match(TokenKind.lParen),
        _PraseParameters(),
        _Match(TokenKind.rParen),
        _ParseFunctionTypeClause(),
        _Match(TokenKind.colon),
        _Match(TokenKind.newLine),
        _ParseBlock());

    Modifiers _ParseModifiers() => new Modifiers(
        _ParseOptional(TokenKind.@static),
        _ParseOptional(TokenKind.mutable));

    // TODO: parse global variables
    GlobalMember _ParseGlobal() => _ParseGlobalFunction();

    GlobalFunction _ParseGlobalFunction() => new GlobalFunction(
        _Match(TokenKind.global),
        _Match(TokenKind.function),
        _ParseIdentifier(),
        _Match(TokenKind.lParen),
        _PraseParameters(),
        _Match(TokenKind.rParen),
        _ParseFunctionTypeClause(),
        _Match(TokenKind.colon),
        _Match(TokenKind.newLine),
        _ParseBlock());

    Token _ParseOptional(TokenKind kind) 
    {
        if (_current.kind == kind)
            return _Next();
        
        return _current.EmptyPoint();
    }

    ImmutableArray<Parameter> _PraseParameters() => _ParseUntil(TokenKind.rParen, _ParseParameter);

    Parameter _ParseParameter() => new Parameter(
        _ParseModifiers(),
        _Match(TokenKind.var),
        _ParseIdentifier(),
        _ParseTypeClause());

    TypeClause _ParseTypeClause() => new TypeClause(
        _Match(TokenKind.colon),
        _ParseName());

    TypeClause _ParseFunctionTypeClause() => new TypeClause(
        _Match(TokenKind.rArrow),
        _ParseName());

    Block _ParseBlock()
    {
        var statements = new List<Statement>();
        var start = _current.location;

        _Match(TokenKind.beginBlock);

        while(_current.kind != TokenKind.endBlock)
        {
            if (_current.kind == TokenKind.newLine)
            {
               _Next();
               continue;
            }

            statements.Add(_ParseStatement());
        }

        return new Block(
            statements.ToImmutableArray(), 
            Location.Embrace(start, _Next().location));
    }

    Statement _ParseStatement()
    {
        switch (_current.kind)
        {
            case TokenKind.mutable:
            case TokenKind.var:
                return _ParsesDeclarationStatement();
            case TokenKind.@return:
                return _ParseReturnStatement();
            case TokenKind.@if:
                return _ParseIfStatement();
            case TokenKind.@while:
                return _ParseWhileStatement();
            case TokenKind.@continue:
            case TokenKind.@break:
                return _ParseFlowControlStatement();
            default:
                return _ParseExpressionStatement();
        }
    }

    DeclarationStatement _ParsesDeclarationStatement() => new DeclarationStatement(
        _ParseModifiers(),
        _Match(TokenKind.var),
        _ParseIdentifier(),
        _ParseTypeClause(),
        _Match(TokenKind.equal),
        _ParseExpression(),
        _Match(TokenKind.semicolon),
        _Match(TokenKind.newLine));

    ReturnStatement _ParseReturnStatement() => new ReturnStatement(
        _Match(TokenKind.@return),
        _ParseExpression(),
        _Match(TokenKind.semicolon),
        _Match(TokenKind.newLine));

    IfStatement _ParseIfStatement() => new IfStatement(
        _Match(TokenKind.@if),
        _Match(TokenKind.lParen),
        _ParseExpression(),
        _Match(TokenKind.rParen),
        _Match(TokenKind.colon),
        _Match(TokenKind.newLine),
        _ParseBlock(),
        _ParseElseStatement());

    Statement _ParseElseStatement()
    {
        if (_current.kind != TokenKind.@else)
            return new EmptyStatement(_current.location.Point());

        var @else = _Next();

        if (_current.kind == TokenKind.@if)
            return _ParseIfStatement();
        
        return new ElseStatement(
            @else,
            _Match(TokenKind.colon),
            _Match(TokenKind.newLine),
            _ParseBlock());
    }

    WhileStatement _ParseWhileStatement() => new WhileStatement(
        _Match(TokenKind.@while),
        _Match(TokenKind.lParen),
        _ParseExpression(),
        _Match(TokenKind.rParen),
        _Match(TokenKind.colon),
        _Match(TokenKind.newLine),
        _ParseBlock());

    FlowControlStatement _ParseFlowControlStatement()
    {
        var statements = new List<Token>();
        
        while (_current.kind == TokenKind.@continue || _current.kind == TokenKind.@break)
            statements.Add(_Next());

        return new FlowControlStatement(
            statements.ToImmutableArray(), 
            _Match(TokenKind.semicolon),
            _Match(TokenKind.newLine));
    }

    ExpressionStatement _ParseExpressionStatement() => new ExpressionStatement(
        _ParseExpression(),
        _Match(TokenKind.semicolon),
        _Match(TokenKind.newLine));

    Expression _ParseExpression() => _ParseBinaryExpression(SyntaxInfo.maxOperatorPrecedence);

    Expression _ParseBinaryExpression(int currentPrecedence)
    {
        if (currentPrecedence == 0)
            return _ParsePrefix();
 
        var left = _ParseBinaryExpression(currentPrecedence - 1);

        while (SyntaxInfo.GetOperatorPrecedence(_current.kind) == currentPrecedence) 
            left = new BinaryExpression(
                left,
                _Next(),
                _ParseBinaryExpression(currentPrecedence - 1));

        return left;
    }

    Expression _ParsePrefix()
    {
        switch (_current.kind)
        {
            case TokenKind.plus:
            case TokenKind.minus:
            case TokenKind.not:
            case TokenKind.plusPlus:
            case TokenKind.minusMinus:
                return new Prefix(_Next(), _ParsePrefix());
        }

        return _ParsePostFix();
    }

    Expression _ParsePostFix()
    {
        var expr = _ParsePrimary();

        while (true)
            switch (_current.kind)
            {
                case TokenKind.lParen:
                    expr = _ParseCall(expr);
                    break;
                case TokenKind.lSquare:
                    expr = _ParseSubscript(expr);
                    break;
                case TokenKind.dot:
                    expr = _ParseMemberAccess(expr);
                    break;
                case TokenKind.plusPlus:
                case TokenKind.minusMinus:
                    expr = new Postfix(expr, _Next());
                    break;
                default:
                    return expr;
            }
    }

    ImmutableArray<Expression> _ParseArguments() => _ParseUntil(TokenKind.rParen, _ParseExpression);

    FunctionCall _ParseCall(Expression expr) => new FunctionCall(
        expr,
        _Match(TokenKind.lParen),
        _ParseArguments(),
        _Match(TokenKind.rParen));

    Subscript _ParseSubscript(Expression expr) => new Subscript (
        expr,
        _Match(TokenKind.lSquare),
        _ParseUntil(TokenKind.rSquare, _ParseExpression),
        _Match(TokenKind.rSquare));

    MemberAccess _ParseMemberAccess(Expression expr) => new MemberAccess (
        expr,
        _Match(TokenKind.dot),
        (ValueToken)_Match(TokenKind.identifier));

    Expression _ParsePrimary()
    {
        switch (_current.kind)
        {
            case TokenKind.number:
                return new Number((ValueToken)_Next());
            case TokenKind.@string:
                return new String((ValueToken)_Next());
            case TokenKind.identifier:
                return _ParseIdentifier();
            case TokenKind.lParen:
                _Next();
                var expr = _ParseExpression();
                _Match(TokenKind.rParen);
                return expr;
        }

        // TODO: diagnostics
        throw new NotImplementedException();
    }

    internal CompilationUnit Analyze() => _ParseCompilationUnit();
}