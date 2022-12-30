namespace Nyx.CodeAnalysis;

using Diagnostics;

class SyntaxAnalyzer : Analyzer<SyntaxNode, SyntaxNode>
{
    public DiagnosticCollection diagnostics;
    SyntaxDefinition _syntax;
    SyntaxNode _currentToken { get => _Peek(0); }
    int _currentEmptySyntaxPos { get => _currentToken.location.pos + _currentToken.location.len; }
    bool _isCurrentValid;

    public SyntaxAnalyzer(SyntaxDefinition syntax, List<SyntaxNode> tokens) : base(tokens, tokens.Last())
    {
        diagnostics = new DiagnosticCollection();
        _syntax = syntax;
        _isCurrentValid = true;
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        yield return _ParseCompilationUnit();
    }

    private SyntaxNode _MatchToken(params SyntaxKind[] kinds)
    {
        foreach (var kind in kinds)
            if (_currentToken.kind == kind) return _ReadNext();

        // TODO: diagnostics
        throw new NotImplementedException();

        // var ret = new SyntaxNode(kinds[0], _currentToken.location);
        // diagnostics.Add(new Error_UnexpectedToken(_currentToken, kinds));
        // _IncrementPos();
        // return ret;
    }

    private SyntaxNode _ParseCompilationUnit()
    {
        List<SyntaxNode> nodes = new List<SyntaxNode>();

        while (_pos < _values.Count - 1)
            nodes.Add(_ParseTopLevelItem());
        nodes.Add(_MatchToken(SyntaxKind.Token_End));

        return new SyntaxNode(SyntaxKind.Syntax_CompilationUnit, nodes);
    }

    private SyntaxNode _ParseTopLevelItem()
    {
        switch (_currentToken.kind)
        {
            // case SyntaxKind.Keyword_Public:
            // case SyntaxKind.Keyword_Static:
            // case SyntaxKind.Keyword_Abstract:
            // case SyntaxKind.Keyword_Enum:
            // case SyntaxKind.Keyword_Struct:
            case SyntaxKind.Keyword_Function:
                // case SyntaxKind.Keyword_Constructor:
                // case SyntaxKind.Keyword_Operator:
                return _ParseTopLevelMember();
            // case SyntaxKind.Keyword_Section:
            //     return _ParseSection();
            // case SyntaxKind.Keyword_Include:
            //     return _ParseIncludeStatement();
            default:
                // TODO: diagnostics
                throw new NotImplementedException();
                // diagnostics.Add(new Error_InvalidTopLevelItem(_currentToken));
                // return new SyntaxNode(SyntaxKind.Syntax_Error, _ReadNext());
        }
    }

    private SyntaxNode _ParseTopLevelMember()
    {
        var modifiers = _ParseModifiers();

        switch (_currentToken.kind)
        {
            // case SyntaxKind.Keyword_Struct:
            //     return _ParseStructDefinition(modifiers);
            // case SyntaxKind.Keyword_Enum:
            //     return _ParseEnumImplementation(modifiers);
            case SyntaxKind.Keyword_Function:
                return _ParseFunctionImplementation(modifiers);
            // case SyntaxKind.Keyword_Constructor:
            //     throw new NotImplementedException();
            // case SyntaxKind.Keyword_Operator:
            //     throw new NotImplementedException();
            default:
                // TODO: diagnostics
                throw new NotImplementedException();
        }
    }

    // private SyntaxNode _ParseSection()
    // {
    //     return new SyntaxNode
    //     (
    //         SyntaxKind.Syntax_Section,
    //         _MatchToken(SyntaxKind.Keyword_Section),
    //         _MatchToken(SyntaxKind.Token_Identifier),
    //         _MatchToken(SyntaxKind.Token_Semicolon)
    //     );
    // }

    // private SyntaxNode _ParseIncludeStatement()
    // {
    //     return new SyntaxNode
    //     (
    //         SyntaxKind.Syntax_IncludeStatement,
    //         _MatchToken(SyntaxKind.Keyword_Include),
    //         _ParseName(),
    //         _MatchToken(SyntaxKind.Token_Semicolon)
    //     );
    // }

    private SyntaxNode _ParseModifiers()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Modifiers,
            SyntaxNode.EmptySyntax(_currentEmptySyntaxPos)
        // _ParseOptional(SyntaxKind.Keyword_Public),
        // _ParseOptional(SyntaxKind.Keyword_Static)
        // _ParseOptional(SyntaxKind.Keyword_Abstract),
        // _ParseOptional(SyntaxKind.Keyword_Mutable)
        );
    }

    private SyntaxNode _ParseOptional(SyntaxKind kind)
    {
        if (_currentToken.kind == kind)
            return _ReadNext();

        return SyntaxNode.EmptySyntax(_currentEmptySyntaxPos);
    }

    // private SyntaxNode _ParseStructDefinition(SyntaxNode modifiers)
    // {
    //     return new SyntaxNode
    //     (
    //         SyntaxKind.Syntax_StructDefinition,
    //         modifiers,
    //         _MatchToken(SyntaxKind.Keyword_Struct),
    //         _MatchToken(SyntaxKind.Token_Identifier),
    //         _MatchToken(SyntaxKind.Token_Colon),
    //         _ParseStructBlock()
    //     );
    // }

    // private SyntaxNode _ParseEnumImplementation(SyntaxNode modifiers)
    // {
    //     throw new NotImplementedException();
    //     return new SyntaxNode
    //     (
    //        SyntaxKind.Syntax_EnumDefinition,
    //        modifiers,
    //        _MatchToken(SyntaxKind.Keyword_Enum),
    //        _MatchToken(SyntaxKind.Token_Colon)
    //     // TODO: parse enum body
    //     );
    // }

    private SyntaxNode _ParseFunctionImplementation(SyntaxNode modifiers)
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_FunctionImplementation,
            modifiers,
            _MatchToken(SyntaxKind.Keyword_Function),
            _ParseName(),
            _MatchToken(SyntaxKind.Token_LParen),
            _ParseOptionalParameters(),
            _MatchToken(SyntaxKind.Token_RParen),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock()
        );
    }

    // private SyntaxNode _ParseStructBlock()
    // {
    //     var nodes = new List<SyntaxNode>();
    //     nodes.Add(_MatchToken(SyntaxKind.Token_BeginBlock));

    //     while (_currentToken.kind != SyntaxKind.Token_EndBlock)
    //     {
    //         switch (_currentToken.kind)
    //         {
    //             // case SyntaxKind.Keyword_Public:
    //             // case SyntaxKind.Keyword_Static:
    //             // case SyntaxKind.Keyword_Abstract:
    //             // case SyntaxKind.Keyword_Mutable:
    //             // case SyntaxKind.Keyword_Enum:
    //             // case SyntaxKind.Keyword_Struct:
    //             case SyntaxKind.Keyword_Var:
    //             case SyntaxKind.Keyword_Function:
    //             // case SyntaxKind.Keyword_Constructor:
    //             // case SyntaxKind.Keyword_Operator:
    //                 nodes.Add(_ParseStructMember());
    //                 break;
    //             // case SyntaxKind.Keyword_Extend:
    //             //     throw new NotImplementedException();
    //             case SyntaxKind.Keyword_Pass:
    //                 nodes.Add(_ParsePassStatement());
    //                 break;
    //             default:
    //                 // TODO: diagnostics
    //                 throw new NotImplementedException();
    //         }
    //     }

    //     if (nodes.Count <= 1)
    //         // TODO: diagnostics, empty structs musst contain pass
    //         throw new NotImplementedException();

    //     nodes.Add(_MatchToken(SyntaxKind.Token_EndBlock));
    //     return new SyntaxNode
    //     (
    //         SyntaxKind.Syntax_StructBlock,
    //         nodes
    //     );
    // }

    private SyntaxNode _ParsePassStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_PassStatement,
            _MatchToken(SyntaxKind.Keyword_Pass),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseOptionalParameters()
    {
        var nodes = new List<SyntaxNode>();

        while (_currentToken.kind != SyntaxKind.Token_RParen)
        {
            nodes.Add(_ParseName());

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

    private SyntaxNode _ParseBlock()
    {
        var nodes = new List<SyntaxNode>();
        nodes.Add(_MatchToken(SyntaxKind.Token_BeginBlock));

        while (_currentToken.kind != SyntaxKind.Token_EndBlock)
            nodes.Add(_ParseStatement());

        if (nodes.Count <= 1)
            // TODO: diagnostics, empty blocks musst contain pass
            throw new NotImplementedException();

        nodes.Add(_MatchToken(SyntaxKind.Token_EndBlock));
        return new SyntaxNode
        (
            SyntaxKind.Syntax_Block,
            nodes
        );
    }

    private SyntaxNode _ParseStatement()
    {
        switch (_currentToken.kind)
        {
            // case SyntaxKind.Keyword_Mutable:
            case SyntaxKind.Keyword_Var:
                return _ParseVariableDeclarationStatement();
            case SyntaxKind.Keyword_Function:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Return:
                return _ParseReturnStatement();
            case SyntaxKind.Keyword_Pass:
                return _ParsePassStatement();
            case SyntaxKind.Keyword_If:
                return _ParseIfStatement();
            case SyntaxKind.Keyword_Switch:
                // return _ParseSwitchStatement();
                throw new NotImplementedException();
            case SyntaxKind.Keyword_For:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_Do:
                throw new NotImplementedException();
            case SyntaxKind.Keyword_While:
                return _ParseWhileStatement();
            case SyntaxKind.Keyword_Continue:
                return _ParseContinueStatement();
            case SyntaxKind.Keyword_Break:
                return _ParseBreakStatement();
            default:
                return _ParseExpression();
        }
    }

    private SyntaxNode _ParseBreakStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_BreakStatement,
            _MatchToken(SyntaxKind.Keyword_Break),
            // TODO: manage double breaks
            _MatchToken(SyntaxKind.Token_Colon)
        );
    }
    
    private SyntaxNode _ParseContinueStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ContinueStatement,
            _MatchToken(SyntaxKind.Keyword_Continue),
            _MatchToken(SyntaxKind.Token_Colon)
        );
    }

    private SyntaxNode _ParseWhileStatement()
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

    private SyntaxNode _ParseVariableDeclarationStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_VariableDeclarationStatement,
            _ParseModifiers(),
            _MatchToken(SyntaxKind.Keyword_Var),
            _MatchToken(SyntaxKind.Token_Identifier),
            // TODO: parse optional type clause
            // TODO: make assignment optional maybe?
            _MatchToken(SyntaxKind.Token_Equal),
            _ParseExpression()
        );
    }

    private SyntaxNode _ParseSwitchStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_SwitchStatement,
            _MatchToken(SyntaxKind.Keyword_Switch),
            _ParseSwitchBlock()
        );
    }

    private SyntaxNode _ParseSwitchBlock()
    {
        var nodes = new List<SyntaxNode>();
        nodes.Add(_MatchToken(SyntaxKind.Token_BeginBlock));

        while (_currentToken.kind != SyntaxKind.Token_EndBlock)
        {
            switch (_currentToken.kind)
            {
                case SyntaxKind.Keyword_Case:
                case SyntaxKind.Keyword_Default:
                    throw new NotImplementedException();
                case SyntaxKind.Keyword_Pass:
                    nodes.Add(_ParsePassStatement());
                    break;
                default:
                    // TODO: diagnostics
                    throw new NotImplementedException();
            }
        }

        if (nodes.Count <= 1)
            // TODO: diagnostics, empty structs musst contain pass
            throw new NotImplementedException();

        nodes.Add(_MatchToken(SyntaxKind.Token_EndBlock));
        return new SyntaxNode
        (
            SyntaxKind.Syntax_SwitchBlock,
            nodes
        );
    }

    private SyntaxNode _ParseIfStatement()
    {
        return new SyntaxNode
        (
            SyntaxKind.Syntax_IfStatement,
            _MatchToken(SyntaxKind.Keyword_If),
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Colon),
            _ParseBlock(),
            _ParseOptionalElse()
        );
    }

    private SyntaxNode _ParseOptionalElse()
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

    private SyntaxNode _ParseReturnStatement()
    {
        // TODO: parse return error
        return new SyntaxNode
        (
            SyntaxKind.Syntax_ReturnStatement,
            _ParseExpression(),
            _MatchToken(SyntaxKind.Token_Semicolon)
        );
    }

    private SyntaxNode _ParseName()
    {
        // TODO: implement parse name the right way
        return _MatchToken(SyntaxKind.Token_Identifier);
    }

    // private SyntaxNode _ParseStructMember()
    // {
    //     var modifiers = _ParseModifiers();

    //     switch (_currentToken.kind)
    //     {
    //         // case SyntaxKind.Keyword_Enum:
    //         // case SyntaxKind.Keyword_Struct:
    //         case SyntaxKind.Keyword_Var:
    //         case SyntaxKind.Keyword_Function:
    //         // case SyntaxKind.Keyword_Constructor:
    //         // case SyntaxKind.Keyword_Operator:
    //             throw new NotImplementedException();
    //         // case SyntaxKind.Keyword_Extend:
    //         //     return _ParseExtend();
    //         default:
    //             // TODO: diagnostics
    //             throw new NotImplementedException();
    //     }

    // }

    // private SyntaxNode _ParseExtend()
    // {
    //     return new SyntaxNode
    //     (
    //         SyntaxKind.Syntax_Extend,
    //         _MatchToken(SyntaxKind.Keyword_Extend),
    //         // TODO: parse name
    //         _MatchToken(SyntaxKind.Token_Identifier),
    //         _MatchToken(SyntaxKind.Token_Semicolon)
    //     );
    // }

    private SyntaxNode _ParseExpression()
    {
        return _ParseBinaryExpression();
    }

    private SyntaxNode _ParseBinaryExpression(int currentPrecedence = 1)
    {
        if (currentPrecedence > _syntax.maxOperatorPrecedence)
            return _ParsePrefix();

        var left = _ParseBinaryExpression(currentPrecedence + 1);

        while (_syntax.BinaryOperatorPrecedence(_currentToken.kind) == currentPrecedence)
        {
            left = new SyntaxNode
            (
                SyntaxKind.Syntax_BinaryOperation,
                left,
                _ReadNext(),
                _ParseBinaryExpression(currentPrecedence + 1)
            );
        }

        return left;
    }

    private SyntaxNode _ParsePrefix()
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

    private SyntaxNode _ParsePostfix()
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
                        _ParseFunctionArguments(),
                        _MatchToken(SyntaxKind.Token_RParen)
                    );
                    break;
                case SyntaxKind.Token_LSquare:
                    expr = new SyntaxNode
                    (
                        SyntaxKind.Syntax_Subscript,
                        expr,
                        _MatchToken(SyntaxKind.Token_LSquare),
                        _ParseSubscriptArguments(),
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

    private SyntaxNode _ParsePrimary()
    {
        switch (_currentToken.kind)
        {
            case SyntaxKind.Token_Number:
            case SyntaxKind.Token_String:
            case SyntaxKind.Token_Identifier:
                return _ReadNext();
        }

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Error,
            _ReadNext()
        );
    }

    // TODO: maybe merge this somehow with _ParseSubscriptArguments
    private SyntaxNode _ParseFunctionArguments()
    {
        var nodes = new List<SyntaxNode>();
        var isArgRemaining = _currentToken.kind != SyntaxKind.Token_RParen;

        while (isArgRemaining)
        {
            // TODO: diagnostics
            _ParseArgument();

            isArgRemaining = _currentToken.kind == SyntaxKind.Token_Comma;
            if (!isArgRemaining) break;

            nodes.Add(_MatchToken(SyntaxKind.Token_Comma));
        }

        if (nodes.Count == 0)
            nodes.Add(SyntaxNode.EmptySyntax(_currentEmptySyntaxPos));

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Arguments,
            nodes
        );
    }

    private SyntaxNode _ParseSubscriptArguments()
    {
        var nodes = new List<SyntaxNode>();
        var isArgRemaining = _currentToken.kind != SyntaxKind.Token_RParen;

        while (isArgRemaining)
        {
            // TODO: diagnostics
            nodes.Add(new SyntaxNode(SyntaxKind.Syntax_Argument, _ParseExpression()));

            isArgRemaining = _currentToken.kind == SyntaxKind.Token_Comma;
            if (!isArgRemaining) break;

            nodes.Add(_MatchToken(SyntaxKind.Token_Comma));
        }

        if (nodes.Count == 0)
            // TODO: diagnostics
            throw new NotImplementedException();

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Arguments,
            nodes
        );
    }

    private SyntaxNode _ParseArgument()
    {
        var expression = _ParseExpression();

        // TODO: implement optional arguments
        // if (expression.kind == SyntaxKind.Token_Identifier &&
        //     _currentToken.kind == SyntaxKind.Token_Colon)
        //     return new SyntaxNode
        //     (
        //         SyntaxKind.Syntax_OptionalArgument,
        //         expression,
        //         _ReadNext(),
        //         _ParseExpression()
        //     );

        return new SyntaxNode
        (
            SyntaxKind.Syntax_Argument,
            expression
        );
    }
}