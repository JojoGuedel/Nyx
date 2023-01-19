namespace Nyx.Analysis;

using Diagnostics;

class SyntaxAnalyzer : Analyzer<SyntaxNode, SyntaxNode>
{
    SyntaxDefinition _syntax;
    SyntaxNode _currentToken { get => _Peek(0); }
    int _currentEmptySyntaxPos { get => _currentToken.location.pos + _currentToken.location.len; }
    bool _isCurrentValid;

    public SyntaxAnalyzer(SyntaxDefinition syntax, List<SyntaxNode> tokens) : base(tokens, tokens.Last())
    {
        _syntax = syntax;
        _isCurrentValid = true;
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        yield return _ParseCompilationUnit();
    }

    SyntaxNode _MatchToken(params SyntaxKind[] kinds)
    {
        foreach (var kind in kinds)
        {
            if (_currentToken.kind == kind) 
            {
                _isCurrentValid = true;

                return _ReadNext();
            }
        }

        if (!_isCurrentValid)
            return new SyntaxNode(kinds[0], _currentToken.location);

        _isCurrentValid = false;

        var ret = new SyntaxNode(kinds[0], _currentToken.location);
        diagnostics.Add(new UnexpectedToken(_ReadNext(), kinds));
        return ret;
    }

    SyntaxNode _ParseCompilationUnit()
    {
        List<SyntaxNode> nodes = new List<SyntaxNode>();

        while (_pos < _values.Count - 1)
            nodes.Add(_ParseTopLevelItem());
        nodes.Add(_MatchToken(SyntaxKind.Token_End));

        return new SyntaxNode(SyntaxKind.Syntax_CompilationUnit, nodes);
    }

    SyntaxNode _ParseTopLevelItem()
    {
        var modifiers = _ParseModifiers();

        switch (_currentToken.kind)
        {
            case SyntaxKind.Keyword_Function:
                return _ParseFunctionImplementation(modifiers);
            default:
                diagnostics.Add(new InvalidToplevelItem(_currentToken));
                return new SyntaxNode(SyntaxKind.Syntax_Error, _ReadNext());
        }
    }

    SyntaxNode _ParseModifiers()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Modifiers,
            _ParseOptional(SyntaxKind.Keyword_Static),
            _ParseOptional(SyntaxKind.Keyword_Mutable)
        );
    }

    SyntaxNode _ParseOptional(SyntaxKind kind)
    {
        if (_currentToken.kind == kind)
            return _ReadNext();

        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    SyntaxNode _ParseName()
    {
        // TODO: implement parse name the right way
        return _MatchToken(SyntaxKind.Token_Literal);
    }

    SyntaxNode _ParseFunctionImplementation(SyntaxNode modifiers)
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_FunctionImplementation,
            modifiers,
            _MatchToken(SyntaxKind.Keyword_Function),
            _ParseName(),
            _MatchToken(SyntaxKind.Token_LParen),
            _ParseParameters(),
            _MatchToken(SyntaxKind.Token_RParen),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock()
        );
    }

    SyntaxNode _ParseParameters()
    {
        var nodes = new List<SyntaxNode>();

        while (_currentToken.kind != SyntaxKind.Token_RParen)
        {
            nodes.Add(_ParseParameter());

            if (_currentToken.kind != SyntaxKind.Token_Comma)
                break;

            nodes.Add(_ReadNext());
        }

        if (nodes.Count == 0)
            nodes.Add(SyntaxNode.EmptySyntax(_currentEmptySyntaxPos));

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Parameters,
            nodes
        );
    }

    SyntaxNode _ParseParameter()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Parameter,
            _ParseModifiers(),
            _MatchToken(SyntaxKind.Keyword_Var),
            _MatchToken(SyntaxKind.Token_Literal),
            _ParseTypeClause()
        );
    }

    SyntaxNode _ParseTypeClause()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_TypeClause,
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseName()
        );
    }

    SyntaxNode _ParseBlock()
    {
        var nodes = new List<SyntaxNode>();
        nodes.Add(_MatchToken(SyntaxKind.Token_BeginBlock));

        while (_currentToken.kind != SyntaxKind.Token_EndBlock)
            nodes.Add(_ParseStatement());

        if (nodes.Count <= 1)
            diagnostics.Add(new EmptyBlock(_currentToken.location));

        nodes.Add(_MatchToken(SyntaxKind.Token_EndBlock));
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Block,
            nodes
        );
    }

    SyntaxNode _ParseStatement()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Keyword_Mutable:
            case SyntaxKind.Keyword_Var:
                return _ParseVariableDeclarationStatement();
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
    
    SyntaxNode _ParseVariableDeclarationStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_VariableDeclarationStatement,
            _ParseModifiers(),
            _MatchToken(SyntaxKind.Keyword_Var),
            _MatchToken(SyntaxKind.Token_Literal),
            // TODO: parse optional type clause
            _ParseTypeClause(),
            // TODO: make assignment optional maybe?
            _MatchToken(SyntaxKind.Token_Equal),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }
    
    SyntaxNode _ParseReturnStatement()
    {
        // TODO: parse, return error
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ReturnStatement,
            _MatchToken(SyntaxKind.Keyword_Return),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }
    
    SyntaxNode _ParseIfStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_IfStatement,
            _MatchToken(SyntaxKind.Keyword_If),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock(),
            _ParseElse()
        );
    }

    SyntaxNode _ParseElse()
    {
        if (_currentToken.kind != SyntaxKind.Keyword_Else)
            return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);

        if (_Peek(1).kind == SyntaxKind.Keyword_If)
            return new SyntaxNode
            (
                SyntaxKind.Syntax_ElseIfStatement,
                _ReadNext(),
                _ParseIfStatement()
            );

        return new SyntaxNode
        (
            SyntaxKind.Syntax_ElseStatement,
            _ReadNext(),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock()
        );
    }
    
    SyntaxNode _ParseWhileStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_WhileStatement,
            _MatchToken(SyntaxKind.Keyword_While),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock()
        );
    }
        
    SyntaxNode _ParseContinueStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ContinueStatement,
            _MatchToken(SyntaxKind.Keyword_Continue),
            _MatchToken(SyntaxKind.Token_Colon)
        );
    }

    SyntaxNode _ParseBreakStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_BreakStatement,
            _MatchToken(SyntaxKind.Keyword_Break),
            // TODO: manage double breaks
            _MatchToken(SyntaxKind.Token_Colon)
        );
    }

    SyntaxNode _ParseSwitchStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_SwitchStatement,
            _MatchToken(SyntaxKind.Keyword_Switch),
            _ParseSwitchBlock()
        );
    }

    SyntaxNode _ParseSwitchBlock()
    {
        var nodes = new List<SyntaxNode>();
        nodes.Add(_MatchToken(SyntaxKind.Token_BeginBlock));

        while (_currentToken.kind != SyntaxKind.Token_EndBlock)
        {
            switch (_currentToken.kind)
            {
                case SyntaxKind.Keyword_Case:
                    return _ParseCaseStatement();
                case SyntaxKind.Keyword_Default:
                    return _ParseDefaultStatement();
                default:
                    diagnostics.Add(new InvalidSwitchBlock(_currentToken));
                    return new SyntaxNode(SyntaxKind.Syntax_Error, _ReadNext());
            }
        }

        if (nodes.Count <= 1)
            diagnostics.Add(new EmptyBlock(_currentToken.location));

        nodes.Add(_MatchToken(SyntaxKind.Token_EndBlock));
        return new SyntaxNode
        (
            SyntaxKind.Syntax_SwitchBlock,
            nodes
        );
    }

    SyntaxNode _ParseCaseStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_CaseStatement,
            _MatchToken(SyntaxKind.Keyword_Case),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock()
        );
    }

    SyntaxNode _ParseDefaultStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_DefaultStatement,
            _MatchToken(SyntaxKind.Keyword_Default),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock()
        );
    }

    SyntaxNode _ParseExpressionStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ExpressionStatement,
            _ParseBinaryExpression(_syntax.maxOperatorPrecedence),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    SyntaxNode _ParseExpression()
    {
        return _ParseBinaryExpression(_syntax.maxOperatorPrecedence);
    }

    SyntaxNode _ParseBinaryExpression(int currentPrecedence)
    {
        if (currentPrecedence == 0)
            return _ParsePrefix();
 
        var left = _ParseBinaryExpression(currentPrecedence - 1);

        while (_syntax.BinaryOperatorPrecedence(_currentToken.kind) == currentPrecedence)
        {
            left = new SyntaxNode
            (
                SyntaxKind.Syntax_BinaryOperation,
                left,
                _ReadNext(),
                _ParseBinaryExpression(currentPrecedence - 1)
            );
        }

        return left;
    }

    SyntaxNode _ParsePrefix()
    {
        var syntaxKind = SyntaxKind.Syntax_Empty;

        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Plus:
                syntaxKind = SyntaxKind.Syntax_PrefixPlus;
                break;
            case SyntaxKind.Token_Minus:
                syntaxKind = SyntaxKind.Syntax_PrefixMinus;
                break;
            case SyntaxKind.Keyword_Not:
                syntaxKind = SyntaxKind.Syntax_PrefixNot;
                break;
            case SyntaxKind.Token_PlusPlus:
                syntaxKind = SyntaxKind.Syntax_PrefixPlusPlus;
                break;
            case SyntaxKind.Token_MinusMinus:
                syntaxKind = SyntaxKind.Syntax_PrefixMinusMinus;
                break;
        }

        if (syntaxKind == SyntaxKind.Syntax_Empty)
            return _ParsePostfix();

        return new SyntaxNode
        (
            syntaxKind,
            _ReadNext(),
            _ParsePrefix()
        );
    }

    SyntaxNode _ParsePostfix()
    {
        var expr = _ParsePrimary();
        var isPostfix = true;

        while (isPostfix)
        {
            switch (_currentToken.kind)
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
                    isPostfix = false;
                    break;
            }
        }

        return expr;
    }

    SyntaxNode _ParsePrimary()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Number:
            case SyntaxKind.Token_String:
            case SyntaxKind.Token_Literal:
                return _ReadNext();
        }

        diagnostics.Add(new InvalidStatement(_currentToken));

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Error,
            _ReadNext()
        );
    }

    SyntaxNode _ParseArguments(SyntaxKind kind, SyntaxKind terminator)
    {
        var nodes = new List<SyntaxNode>();
        
        while (_currentToken.kind != terminator)
        {
            nodes.Add(new SyntaxNode(SyntaxKind.Syntax_Argument, _ParseExpression()));

            if (_currentToken.kind != SyntaxKind.Token_Comma)
                break;

            nodes.Add(_ReadNext());
        }

        if (nodes.Count == 0)
            nodes.Add(SyntaxNode.EmptySyntax(_currentEmptySyntaxPos));

        return new SyntaxNode
        (
            kind,
            nodes
        );
    }

    SyntaxNode _ParseArgument()
    {
        // TODO: implement optional arguments
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Argument,
            _ParseExpression()
        );
    }
}