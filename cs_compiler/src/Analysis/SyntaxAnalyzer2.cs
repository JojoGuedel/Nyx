using System.Collections.Immutable;
using Nyx.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class SyntaxAnalyzer2 : Analyzer<LexerNode, Node>
{
    SyntaxDefinition _syntax; 
    // TODO: probably change this
    public SyntaxAnalyzer2(SyntaxDefinition syntax, List<LexerNode> values) : 
        base(values, values.Last())
    {
        _syntax = syntax;
    }

    public override IEnumerable<Node> GetAll()
    {
        throw new NotImplementedException();
    }

    LexerNode _Match(SyntaxKind kind)
    {
        if (_current.kind == kind)
            return _ReadNext();
        
        throw new NotImplementedException();
        // TODO: adjust diagnostics to new node type
        // diagnostics.Add(new UnexpectedToken(_current, kind));
        // _IncrementPos();
        // return new ErrorNode(_last.location);
    }

    ImmutableArray<T> _ParseUntil<T>(SyntaxKind terminator, Func<T> parseCB, SyntaxKind separator = SyntaxKind.Token_Comma)
    {
        var nodes = new List<T>();
        
        while (_current.kind != terminator)
        {
            nodes.Add(parseCB());

            if (_current.kind != separator)
                break;

            _IncrementPos();
        }

        return nodes.ToImmutableArray();
    }

    Function _ParseFunction() => new Function
    (
        _ParseModifiers(),
        _ParseUntil(SyntaxKind.Token_RParen, _PraseParameter),
        _ParseFunctionTypeClause(),
        _ParseBlock()
    );

    Modifiers _ParseModifiers() => new Modifiers
    (
        _ParseOptional(SyntaxKind.Keyword_Static),
        _ParseOptional(SyntaxKind.Keyword_Mutable)
    );

    Node _ParseOptional(SyntaxKind kind) 
    {
        if (_current.kind == kind)
            return _ReadNext();
        
        return new EmptyNode(_current.location.end);
    }

    Parameter _PraseParameter() => new Parameter
    (
        _ParseModifiers(),
        _Match(SyntaxKind.Keyword_Var),
        _Match(SyntaxKind.Token_Literal),
        _ParseTypeClause()
    );

    // TODO: change this
    LexerNode _ParseName() => _Match(SyntaxKind.Token_Literal);

    TypeClause _ParseTypeClause() => new TypeClause
    (
        _Match(SyntaxKind.Token_Colon),
        _ParseName()
    );

    TypeClause _ParseFunctionTypeClause() => new TypeClause
    (
        _Match(SyntaxKind.Token_RArrow),
        _ParseName()
    );

    Block _ParseBlock()
    {
        var statements = new List<Statement>();
        var start = _current.location;

        _Match(SyntaxKind.Token_BeginBlock);

        while(_current.kind != SyntaxKind.Token_EndBlock)
            statements.Add(_ParseStatement());

        return new Block
        (
            statements.ToImmutableArray(), 
            TextLocation.Embrace(start, _ReadNext().location)
        );
    }

    Statement _ParseStatement()
    {
        switch (_current.kind)
        {
            case SyntaxKind.Keyword_Mutable:
            case SyntaxKind.Keyword_Var:
                return _ParsesDeclarationStatement();
            case SyntaxKind.Keyword_Return:
                return _ParseReturnStatement();
            case SyntaxKind.Keyword_If:
                return _ParseIfStatement();
            case SyntaxKind.Keyword_Switch:
                return _ParseSwitchStatement();
            case SyntaxKind.Keyword_While:
                return _ParseWhileStatement();
            case SyntaxKind.Keyword_Continue:
                return _ParseContinueStatement();
            case SyntaxKind.Keyword_Break:
                return _ParseBreakStatement();
            default:
                return _ParseExpressionStatement();
        }
    }

    DeclarationStatement _ParsesDeclarationStatement() => new DeclarationStatement
    (
        _ParseModifiers(),
        _Match(SyntaxKind.Keyword_Var),
        _ParseName(),
        _ParseTypeClause(),
        _Match(SyntaxKind.Token_Equal),
        _ParseExpression(), 
        _Match(SyntaxKind.Token_Semicolon)
    );

    ExpressionStatement _ParseExpressionStatement() => new ExpressionStatement
    (
        _ParseExpression(),
        _Match(SyntaxKind.Token_Semicolon)
    );

    Expression _ParseExpression()
    {
        return _ParseBinaryExpression(_syntax.maxOperatorPrecedence);
    }

    Expression _ParseBinaryExpression(int currentPrecedence)
    {
        if (currentPrecedence == 0)
            return _ParsePrefix();
 
        var left = _ParseBinaryExpression(currentPrecedence - 1);

        while (_syntax.BinaryOperatorPrecedence(_current.kind) == currentPrecedence)
        {
            left = new BinaryExpression
            (
                left,
                _ReadNext(),
                _ParseBinaryExpression(currentPrecedence - 1)
            );
        }

        return left;
    }

    Expression _ParsePrefix()
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

    Expression _ParsePostFix()
    {
        var expr = _ParsePrimary();

        for (var hasPostfix = true; hasPostfix;)
        {
            switch (_current.kind)
            {
                case SyntaxKind.Token_LParen:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_FunctionCall,
                        expr,
                        _MatchToken(SyntaxKind.Token_LParen),
                        _ParseArguments(SyntaxKind.Syntax_Arguments, SyntaxKind.Token_RParen),
                        _MatchToken(SyntaxKind.Token_RParen)
                    );
                    break;
                case SyntaxKind.Token_LSquare:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_Subscript,
                        expr,
                        _MatchToken(SyntaxKind.Token_LSquare),
                        _ParseArguments(SyntaxKind.Syntax_Subscript, SyntaxKind.Token_RSquare),
                        _MatchToken(SyntaxKind.Token_RSquare)
                    );
                    break;
                case SyntaxKind.Token_PlusPlus:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_PostfixIncrement,
                        expr,
                        _ReadNext()
                    );
                    break;
                case SyntaxKind.Token_MinusMinus:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_PostfixDecrement,
                        expr,
                        _ReadNext()
                    );
                    break;
                default:
                    hasPostfix = false;
                    break;
            }
        }        

        return expr;
    }

    Expression _ParsePrimary()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Number:
            case SyntaxKind.Token_String:
            case SyntaxKind.Token_Literal:
                return _ReadNext();
            case SyntaxKind.Keyword_True:
            case SyntaxKind.Keyword_False:
                break;
            case SyntaxKind.Token_LParen:
                break;                
        }

        diagnostics.Add(new InvalidStatement(_currentToken));

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Error,
            _ReadNext()
        );
    }

    FunctionCall _ParseFunctionCall() => new FunctionCall
    (
        _ParseName(),
        _ParseUntil(SyntaxKind.Token_RParen, _ParseExpression)
    );
}