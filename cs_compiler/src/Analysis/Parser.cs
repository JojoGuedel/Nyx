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

    internal CompilationUnit Analyze() => _ParseCompilationUnit();

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
            case TokenKind.@struct:
            case TokenKind.global:
                throw new NotImplementedException();
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
        _ParseBlock()
    );

    Modifiers _ParseModifiers() => new Modifiers(
        _ParseOptional(TokenKind.@static),
        _ParseOptional(TokenKind.mutable)
    );

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
        _ParseBlock()
    );

    Token _ParseOptional(TokenKind kind) 
    {
        if (_current.kind == kind)
            return _Next();
        
        return _current.EmptyPoint();
    }

    ImmutableArray<Parameter> _PraseParameters() => _ParseUntil(TokenKind.rParen, _ParseParameter)

    Parameter _ParseParameter() => new Parameter(
        _ParseModifiers(),
        _Match(TokenKind.var),
        _ParseIdentifier(),
        _ParseTypeClause()
    );

    TypeClause _ParseTypeClause() => new TypeClause(
        _Match(TokenKind.colon),
        _ParseName()
    );

    TypeClause _ParseFunctionTypeClause() => new TypeClause(
        _Match(TokenKind.rArrow),
        _ParseName()
    );

    Block _ParseBlock()
    {
        var statements = new List<Statement>();
        var start = _current.location;

        _Match(TokenKind.beginBlock);

        while(_current.kind != TokenKind.endBlock)
            statements.Add(_ParseStatement());

        return new Block
        (
            statements.ToImmutableArray(), 
            Location.Embrace(start, _Next().location)
        );
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
        _Match(TokenKind.newLine)
    );

    ReturnStatement _ParseReturnStatement() => new ReturnStatement(
        _Match(TokenKind.@return),
        _ParseExpression(),
        _Match(TokenKind.semicolon),
        _Match(TokenKind.newLine)
    );

    IfStatement _ParseIfStatement() => new IfStatement(
        _Match(TokenKind.@if),
        _Match(TokenKind.lParen),
        _ParseExpression(),
        _Match(TokenKind.rParen),
        _Match(TokenKind.colon),
        _Match(TokenKind.newLine),
        _ParseBlock(),
        _ParseElseStatement()
    );

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
            _ParseBlock()
        );
    }

    WhileStatement _ParseWhileStatement() => new WhileStatement
    (
        _Match(TokenKind.@while),
        _Match(TokenKind.lParen),
        _ParseExpression(),
        _Match(TokenKind.rParen),
        _Match(TokenKind.colon),
        _Match(TokenKind.newLine),
        _ParseBlock()
    );

    FlowControlStatement _ParseFlowControlStatement()
    {
        List<LexerNode> statements = new List<LexerNode>();
        
        while (_current.kind == SyntaxKind.Keyword_Continue || _current.kind == SyntaxKind.Keyword_Break)
            statements.Add(_ReadNext());

        return new FlowControlStatement(statements.ToImmutableArray(), _Match(SyntaxKind.Token_Semicolon));
    }

    ExpressionStatement _ParseExpressionStatement() => new ExpressionStatement
    (
        _ParseExpression(),
        _Match(SyntaxKind.Token_Semicolon)
    );

    _Expression _ParseExpression()
    {
        return _ParseBinaryExpression(_syntax.maxOperatorPrecedence);
    }

    _Expression _ParseBinaryExpression(int currentPrecedence)
    {
        if (currentPrecedence == 0)
            return _ParsePrefix();
 
        var left = _ParseBinaryExpression(currentPrecedence - 1);

        while (_syntax.BinaryOperatorPrecedence(_current.kind) == currentPrecedence) left = new BinaryExpression
        (
            left,
            _ReadNext(),
            _ParseBinaryExpression(currentPrecedence - 1)
        );

        return left;
    }

    _Expression _ParsePrefix()
    {
        switch (_current.kind)
        {
            case SyntaxKind.Token_Plus:
            case SyntaxKind.Token_Minus:
            case SyntaxKind.Keyword_Not:
            case SyntaxKind.Token_PlusPlus:
            case SyntaxKind.Token_MinusMinus:
                return new Prefix(_ReadNext(), _ParsePrefix());
        }

        return _ParsePostFix();
    }

    _Expression _ParsePostFix()
    {
        var expr = _ParsePrimary();

        while(true)
        {
            switch (_current.kind)
            {
                case SyntaxKind.Token_LParen:
                    expr = _ParseCall(expr);
                    break;
                case SyntaxKind.Token_LSquare:
                    expr = _ParseSubscript(expr);
                    break;
                case SyntaxKind.Token_Dot:
                    expr = _ParseMemberAccess(expr);
                    break;
                case SyntaxKind.Token_PlusPlus:
                case SyntaxKind.Token_MinusMinus:
                    expr = new Postfix(expr, _ReadNext());
                    break;
                default:
                    return expr;
            }
        }
    }

    FunctionCall _ParseCall(_Expression expr) => new FunctionCall
    (
        expr,
        _Match(SyntaxKind.Token_LParen),
        _ParseUntil(SyntaxKind.Token_RParen, _ParseExpression),
        _Match(SyntaxKind.Token_RParen)
    );

    Subscript _ParseSubscript(_Expression expr) => new Subscript
    (
        expr,
        _Match(SyntaxKind.Token_LSquare),
        _ParseUntil(SyntaxKind.Token_RSquare, _ParseExpression),
        _Match(SyntaxKind.Token_RSquare)
    );

    MemberAccess _ParseMemberAccess(Expression expr) => new MemberAccess
    (
        expr,
        _Match(TokenKind.dot),
        (ValueToken)_Match(TokenKind.identifier)
    );

    _Expression _ParsePrimary()
    {
        switch (_current.kind)
        {
            case SyntaxKind.Token_Number:
                return new Number(_ReadNext());
            case SyntaxKind.Token_String:
                return new String(_ReadNext());
            case SyntaxKind.Token_Identifier:
                return new Identifier(_ReadNext());
            case SyntaxKind.Token_LParen:
                _ReadNext();
                var expr = _ParseExpression();
                _Match(SyntaxKind.Token_RParen);
                return expr;
        }

        // TODO: diagnostics
        throw new NotImplementedException();
    }
}

// public class _Parser : Analyzer<LexerNode, _Node>
// {
//     _SyntaxInfo _syntax; 
//     // TODO: probably change this
//     public _Parser(_SyntaxInfo syntax, List<LexerNode> values) : 
//         base(values, values.Last())
//     {
//         _syntax = syntax;
//     }

//     public override IEnumerable<_Node> Analyze()
//     {
//         yield return _ParseCompilationUnit();
//     }

//     LexerNode _Match(SyntaxKind kind)
//     {
//         if (_current.kind == kind)
//             return _ReadNext();
        
//         // throw new NotImplementedException();
//         // TODO: adjust diagnostics to new node type
//         // diagnostics.Add(new UnexpectedToken(_current, kind));
//         return new ErrorNode(_ReadNext());
//     }

//     ImmutableArray<T> _ParseUntil<T>(SyntaxKind terminator, Func<T> parseCB, SyntaxKind separator = SyntaxKind.Token_Comma)
//     {
//         var nodes = new List<T>();
        
//         while (_current.kind != terminator)
//         {
//             nodes.Add(parseCB());

//             if (_current.kind != separator)
//                 break;

//             _IncrementPos();
//         }

//         return nodes.ToImmutableArray();
//     }

//     Identifier _ParseIdentifier() => new Identifier(_Match(SyntaxKind.Token_Identifier));
//     // TODO: change this

//     Expression _ParseName()
//     {
//         Expression expr = _ParseIdentifier();

//         while (_current.kind == SyntaxKind.Token_Dot)
//             expr = _ParseMemberAccess(expr);

//         return expr;
//     }

//     _CompilationUnit _ParseCompilationUnit()
//     {
//         var members = new List<_Member>();

//         // TODO: check if there are no endless loops
//         while (_current.kind != SyntaxKind.Token_End)
//             members.Add(_ParseMember());
        
//         return new _CompilationUnit(members.ToImmutableArray(), _ReadNext());
//     }

//     // TODO: parse structs
//     _Member _ParseMember()
//     {
//         switch(_current.kind)
//         {
//             case SyntaxKind.Keyword_Function:
//                 return _ParseFunction();
//             case SyntaxKind.Keyword_Struct:
//                 throw new NotImplementedException();
//             case SyntaxKind.Keyword_Global:
//                 return _ParseGlobal();
//             default: 
//                 // TODO: add diagnostics
//                 throw new NotImplementedException();
//         }
//     }

//     Function _ParseFunction() => new Function
//     (
//         _ParseModifiers(),
//         _Match(SyntaxKind.Keyword_Function),
//         _ParseName(),
//         _Match(SyntaxKind.Token_LParen),
//         _ParseUntil(SyntaxKind.Token_RParen, _PraseParameter),
//         _Match(SyntaxKind.Token_RParen),
//         _ParseFunctionTypeClause(),
//         _Match(SyntaxKind.Token_Colon),
//         _ParseBlock()
//     );

//     // TODO: parse global variables
//     GlobalMember _ParseGlobal() => _ParseGlobalFunction();

//     GlobalFunction _ParseGlobalFunction() => new GlobalFunction
//     (
//         _Match(SyntaxKind.Keyword_Global),
//         _Match(SyntaxKind.Keyword_Function),
//         _ParseIdentifier(),
//         _Match(SyntaxKind.Token_LParen),
//         _ParseUntil(SyntaxKind.Token_RParen, _PraseParameter),
//         _Match(SyntaxKind.Token_RParen),
//         _ParseFunctionTypeClause(),
//         _Match(SyntaxKind.Token_Colon),
//         _ParseBlock()
//     );

//     Modifiers _ParseModifiers() => new Modifiers
//     (
//         _ParseOptional(SyntaxKind.Keyword_Static),
//         _ParseOptional(SyntaxKind.Keyword_Mutable)
//     );

//     _Node _ParseOptional(SyntaxKind kind) 
//     {
//         if (_current.kind == kind)
//             return _ReadNext();
        
//         return new EmptyNode(_current.location.pos);
//     }

//     Parameter _PraseParameter() => new Parameter
//     (
//         _ParseModifiers(),
//         _Match(SyntaxKind.Keyword_Var),
//         _Match(SyntaxKind.Token_Identifier),
//         _ParseTypeClause()
//     );

//     TypeClause _ParseTypeClause() => new TypeClause
//     (
//         _Match(SyntaxKind.Token_Colon),
//         _ParseName()
//     );

//     TypeClause _ParseFunctionTypeClause() => new TypeClause
//     (
//         _Match(SyntaxKind.Token_RArrow),
//         _ParseName()
//     );

//     Block _ParseBlock()
//     {
//         var statements = new List<Statement>();
//         var start = _current.location;

//         _Match(SyntaxKind.Token_BeginBlock);

//         while(_current.kind != SyntaxKind.Token_EndBlock)
//             statements.Add(_ParseStatement());

//         return new Block
//         (
//             statements.ToImmutableArray(), 
//             TextLocation.Embrace(start, _ReadNext().location)
//         );
//     }

//     Statement _ParseStatement()
//     {
//         switch (_current.kind)
//         {
//             case SyntaxKind.Keyword_Mutable:
//             case SyntaxKind.Keyword_Var:
//                 return _ParsesDeclarationStatement();
//             case SyntaxKind.Keyword_Return:
//                 return _ParseReturnStatement();
//             case SyntaxKind.Keyword_If:
//                 return _ParseIfStatement();
//             case SyntaxKind.Keyword_While:
//                 return _ParseWhileStatement();
//             case SyntaxKind.Keyword_Continue:
//             case SyntaxKind.Keyword_Break:
//                 return _ParseFlowControlStatement();
//             default:
//                 return _ParseExpressionStatement();
//         }
//     }

//     DeclarationStatement _ParsesDeclarationStatement() => new DeclarationStatement
//     (
//         _ParseModifiers(),
//         _Match(SyntaxKind.Keyword_Var),
//         _ParseIdentifier(),
//         _ParseTypeClause(),
//         _Match(SyntaxKind.Token_Equal),
//         _ParseExpression(), 
//         _Match(SyntaxKind.Token_Semicolon)
//     );

//     ReturnStatement _ParseReturnStatement() => new ReturnStatement
//     (
//         _Match(SyntaxKind.Keyword_Return),
//         _ParseExpression(),
//         _Match(SyntaxKind.Token_Semicolon)
//     );

//     IfStatement _ParseIfStatement() => new IfStatement
//     (
//         _Match(SyntaxKind.Keyword_If),
//         _ParseExpression(),
//         _ParseBlock(),
//         _ParseElseStatement()
//     );

//     Statement _ParseElseStatement()
//     {
//         if (_current.kind != SyntaxKind.Keyword_Else)
//             return new EmptyStatement(_current.location.pos);

//         _ReadNext();
//         return _ParseIfStatement();
//     }

//     WhileStatement _ParseWhileStatement() => new WhileStatement
//     (
//         _Match(SyntaxKind.Keyword_While),
//         _ParseExpression(),
//         _ParseBlock()
//     );

//     FlowControlStatement _ParseFlowControlStatement()
//     {
//         List<LexerNode> statements = new List<LexerNode>();
        
//         while (_current.kind == SyntaxKind.Keyword_Continue || _current.kind == SyntaxKind.Keyword_Break)
//             statements.Add(_ReadNext());

//         return new FlowControlStatement(statements.ToImmutableArray(), _Match(SyntaxKind.Token_Semicolon));
//     }

//     ExpressionStatement _ParseExpressionStatement() => new ExpressionStatement
//     (
//         _ParseExpression(),
//         _Match(SyntaxKind.Token_Semicolon)
//     );

//     Expression _ParseExpression()
//     {
//         return _ParseBinaryExpression(_syntax.maxOperatorPrecedence);
//     }

//     Expression _ParseBinaryExpression(int currentPrecedence)
//     {
//         if (currentPrecedence == 0)
//             return _ParsePrefix();
 
//         var left = _ParseBinaryExpression(currentPrecedence - 1);

//         while (_syntax.BinaryOperatorPrecedence(_current.kind) == currentPrecedence) left = new BinaryExpression
//         (
//             left,
//             _ReadNext(),
//             _ParseBinaryExpression(currentPrecedence - 1)
//         );

//         return left;
//     }

//     Expression _ParsePrefix()
//     {
//         switch (_current.kind)
//         {
//             case SyntaxKind.Token_Plus:
//             case SyntaxKind.Token_Minus:
//             case SyntaxKind.Keyword_Not:
//             case SyntaxKind.Token_PlusPlus:
//             case SyntaxKind.Token_MinusMinus:
//                 return new Prefix(_ReadNext(), _ParsePrefix());
//         }

//         return _ParsePostFix();
//     }

//     Expression _ParsePostFix()
//     {
//         var expr = _ParsePrimary();

//         while(true)
//         {
//             switch (_current.kind)
//             {
//                 case SyntaxKind.Token_LParen:
//                     expr = _ParseCall(expr);
//                     break;
//                 case SyntaxKind.Token_LSquare:
//                     expr = _ParseSubscript(expr);
//                     break;
//                 case SyntaxKind.Token_Dot:
//                     expr = _ParseMemberAccess(expr);
//                     break;
//                 case SyntaxKind.Token_PlusPlus:
//                 case SyntaxKind.Token_MinusMinus:
//                     expr = new Postfix(expr, _ReadNext());
//                     break;
//                 default:
//                     return expr;
//             }
//         }
//     }

//     FunctionCall _ParseCall(Expression expr) => new FunctionCall
//     (
//         expr,
//         _Match(SyntaxKind.Token_LParen),
//         _ParseUntil(SyntaxKind.Token_RParen, _ParseExpression),
//         _Match(SyntaxKind.Token_RParen)
//     );

//     Subscript _ParseSubscript(Expression expr) => new Subscript
//     (
//         expr,
//         _Match(SyntaxKind.Token_LSquare),
//         _ParseUntil(SyntaxKind.Token_RSquare, _ParseExpression),
//         _Match(SyntaxKind.Token_RSquare)
//     );

//     MemberAccess _ParseMemberAccess(Expression expr) => new MemberAccess
//     (
//         expr,
//         _Match(SyntaxKind.Token_Dot),
//         _Match(SyntaxKind.Token_Identifier)
//     );

//     Expression _ParsePrimary()
//     {
//         switch (_current.kind)
//         {
//             case SyntaxKind.Token_Number:
//                 return new Number(_ReadNext());
//             case SyntaxKind.Token_String:
//                 return new String(_ReadNext());
//             case SyntaxKind.Token_Identifier:
//                 return new Identifier(_ReadNext());
//             case SyntaxKind.Token_LParen:
//                 _ReadNext();
//                 var expr = _ParseExpression();
//                 _Match(SyntaxKind.Token_RParen);
//                 return expr;
//         }

//         // TODO: diagnostics
//         throw new NotImplementedException();
//     }
// }
