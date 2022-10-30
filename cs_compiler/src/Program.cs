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

    var tokenizer = new Tokenizer(input, syntax);
    var tokens = new List<SyntaxToken>();

    while (true)
    {
        var token = tokenizer.GetNextToken();

        tokens.Add(token);

        if (token.Kind == SyntaxTokenKind.END)
            break;
    }

    foreach (var token in tokens)
        Console.WriteLine(token.Kind);
}

void ManageEscapeCommands(string command)
{
    switch (command)
    {
        case "exit":
            running = false;
            break;

        default:
            Console.WriteLine($"Unknown command '{command}'");
            break;
    }
}

SyntaxDefinition GetSyntax()
{
    var syntax = new SyntaxDefinition();

    // syntax.DefineSingleToken('\u0000', SyntaxTokenKind.END);
    // syntax.DefineSingleToken('\n', SyntaxTokenKind.NEWLINE);

    syntax.DefineSingleToken('(', SyntaxTokenKind.LPAREN);
    syntax.DefineSingleToken(')', SyntaxTokenKind.RPAREN);
    syntax.DefineSingleToken('[', SyntaxTokenKind.LSQUAR);
    syntax.DefineSingleToken(']', SyntaxTokenKind.RSQUAR);

    syntax.DefineSingleToken('.', SyntaxTokenKind.DOT);
    syntax.DefineSingleToken(',', SyntaxTokenKind.COMMA);
    syntax.DefineSingleToken(';', SyntaxTokenKind.SEMICOLON);

    syntax.DefineSingleToken('+', SyntaxTokenKind.PLUS);
    syntax.DefineSingleToken('-', SyntaxTokenKind.MINUS);
    syntax.DefineSingleToken('*', SyntaxTokenKind.STAR);
    syntax.DefineSingleToken('/', SyntaxTokenKind.SLASH);

    syntax.DefineSingleToken('<', SyntaxTokenKind.LESS);
    syntax.DefineSingleToken('>', SyntaxTokenKind.GREATER);
    syntax.DefineSingleToken('=', SyntaxTokenKind.EQUAL);
    syntax.DefineSingleToken('%', SyntaxTokenKind.PERCENT);

    syntax.DefineDoubleToken(('=', '='), SyntaxTokenKind.EQUALEQUAL);
    syntax.DefineDoubleToken(('!', '='), SyntaxTokenKind.NOTEQUAL);
    syntax.DefineDoubleToken(('<', '='), SyntaxTokenKind.LESSEQUAL);
    syntax.DefineDoubleToken(('>', '='), SyntaxTokenKind.GREATEREQUAL);
    
    syntax.DefineDoubleToken(('+', '+'), SyntaxTokenKind.PLUSPLUS);
    syntax.DefineDoubleToken(('+', '='), SyntaxTokenKind.PLUSEQUAL);
    syntax.DefineDoubleToken(('-', '-'), SyntaxTokenKind.MINUSMINUS);
    syntax.DefineDoubleToken(('-', '='), SyntaxTokenKind.MINUSEQUAL);
    syntax.DefineDoubleToken(('*', '='), SyntaxTokenKind.STAREQUAL);
    syntax.DefineDoubleToken(('/', '='), SyntaxTokenKind.SLASHEQUAL);
    syntax.DefineDoubleToken(('%', '='), SyntaxTokenKind.PERCENTEQUAL);

    syntax.DefineSingleToken('"', SyntaxTokenKind.STRING);
    syntax.DefineSingleToken('\'', SyntaxTokenKind.STRING);

    return syntax;
}
