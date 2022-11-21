using SyntaxAnalysis;

var running = true;
var syntax = GetSyntax();

while (running)
{
    Console.Write("> ");

    var input = Console.ReadLine();

    if (input is null)
        input=String.Empty;

    if (input.Length >= 1 && input[0] == '#')
    {
        ManageEscapeCommands(input.Substring(1));
        continue;
    }

    var lexer = new Lexer(syntax, input);
    var tokens = lexer.GetAll().ToList();

    // foreach (var token in tokens)
    //     Console.WriteLine(token.kind);

    foreach(var diagnostic in lexer.diagnostics.GetAll())
        Console.WriteLine(diagnostic.GetMessage());

    var indentAnalyzer = new IndentAnalyzer(tokens);
    tokens = indentAnalyzer.GetAll().ToList();

    foreach (var token in tokens)
        Console.WriteLine(token.kind);

    var parser = new Parser(syntax, tokens);
    var compilationUnit = parser.GetAll();

    foreach(var diagnostic in parser.diagnostics.GetAll())
        Console.WriteLine(diagnostic.GetMessage());

    // foreach (var node in compilationUnit.children)
    //     Console.WriteLine(node.kind);
}

void ManageEscapeCommands(string command)
{
    switch (command)
    {
        case "exit":
            running = false;
            break;

        case "clear":
        case "cls":
            Console.Clear();
            break;

        default:
            Console.WriteLine($"Unknown command '{command}'");
            break;
    }
}

SyntaxDefinition GetSyntax()
{
    var syntax = new SyntaxDefinition();

    // syntax.DefineSingleToken('\u0000', SyntaxKind.Token_End);
    // syntax.DefineSingleToken('\n', SyntaxKind.Token_NewLine);

    syntax.DefineSingleToken(' ', SyntaxKind._Discard);

    syntax.DefineSingleToken('"', SyntaxKind._StringMarker);
    syntax.DefineSingleToken('\'', SyntaxKind._StringMarker);

    syntax.DefineSingleToken('(', SyntaxKind.Token_LParen);
    syntax.DefineSingleToken(')', SyntaxKind.Token_RParen);
    syntax.DefineSingleToken('[', SyntaxKind.Token_LSquare);
    syntax.DefineSingleToken(']', SyntaxKind.Token_RSquare);

    syntax.DefineSingleToken('.', SyntaxKind.Token_Dot);
    syntax.DefineSingleToken(',', SyntaxKind.Token_Comma);
    syntax.DefineSingleToken(':', SyntaxKind.Token_Colon);
    syntax.DefineSingleToken(';', SyntaxKind.Token_Semicolon);

    syntax.DefineSingleToken('+', SyntaxKind.Token_Plus);
    syntax.DefineSingleToken('-', SyntaxKind.Token_Minus);
    syntax.DefineSingleToken('*', SyntaxKind.Token_Star);
    syntax.DefineSingleToken('/', SyntaxKind.Token_Slash);

    syntax.DefineSingleToken('<', SyntaxKind.Token_Less);
    syntax.DefineSingleToken('>', SyntaxKind.Token_Greater);
    syntax.DefineSingleToken('=', SyntaxKind.Token_Equal);
    syntax.DefineSingleToken('%', SyntaxKind.Token_Percent);

    syntax.DefineDoubleToken(('=', '='), SyntaxKind.Token_EqualEqual);
    syntax.DefineDoubleToken(('!', '='), SyntaxKind.Token_NotEqual);
    syntax.DefineDoubleToken(('<', '='), SyntaxKind.Token_LessEqual);
    syntax.DefineDoubleToken(('>', '='), SyntaxKind.Token_GreaterEqual);
    
    syntax.DefineDoubleToken(('+', '+'), SyntaxKind.Token_PlusPlus);
    syntax.DefineDoubleToken(('+', '='), SyntaxKind.Token_PlusEqual);
    syntax.DefineDoubleToken(('-', '-'), SyntaxKind.Token_MinusMinus);
    syntax.DefineDoubleToken(('-', '='), SyntaxKind.Token_MinusEqual);
    syntax.DefineDoubleToken(('*', '='), SyntaxKind.Token_StarEqual);
    syntax.DefineDoubleToken(('/', '='), SyntaxKind.Token_SlashEqual);
    syntax.DefineDoubleToken(('%', '='), SyntaxKind.Token_PercentEqual);

    syntax.DefineKeyword("fn", SyntaxKind.Keyword_Funcion);
    syntax.DefineKeyword("let", SyntaxKind.Keyword_Let);
    syntax.DefineKeyword("if", SyntaxKind.Keyword_If);
    syntax.DefineKeyword("else", SyntaxKind.Keyword_Else);
    syntax.DefineKeyword("for", SyntaxKind.Keyword_For);
    syntax.DefineKeyword("do", SyntaxKind.Keyword_Do);
    syntax.DefineKeyword("while", SyntaxKind.Keyword_While);

    return syntax;
}
