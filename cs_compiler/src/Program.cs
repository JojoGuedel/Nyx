using SyntaxAnalysis;

var running = true;
var syntax = GetSyntax();

while (running)
{
    Console.Write("> ");

    var input = Console.ReadLine();

    if (input is null || input.Length < 1)
        continue;

    if (input[0] == '#')
    {
        ManageEscapeCommands(input.Substring(1));
        continue;
    }

    List<SyntaxToken> tokens = new List<SyntaxToken>();
    Lexer lexer = new Lexer(input, syntax);

    while (true)
    {
        var token = lexer.NextToken();

        tokens.Add(token);

        if (token.Kind == SyntaxTokenKind.End)
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

    syntax.Define("\0", SyntaxTokenKind.End);

    syntax.Define("+", SyntaxTokenKind.Plus);
    syntax.Define("-", SyntaxTokenKind.Minus);
    syntax.Define("*", SyntaxTokenKind.Star);
    syntax.Define("/", SyntaxTokenKind.Slash);
    syntax.Define("=", SyntaxTokenKind.Equal);
    syntax.Define("+=", SyntaxTokenKind.PlusEqual);
    syntax.Define("-=", SyntaxTokenKind.MinusEqual);
    syntax.Define("*=", SyntaxTokenKind.StarEqual);
    syntax.Define("/=", SyntaxTokenKind.SlashEqual);
    syntax.Define("==", SyntaxTokenKind.EqualEqual);
    syntax.Define("!=", SyntaxTokenKind.NotEqual);
    syntax.Define("<=", SyntaxTokenKind.LessEqual);
    syntax.Define(">=", SyntaxTokenKind.GreaterEqual);
    syntax.Define("<", SyntaxTokenKind.Less);
    syntax.Define(">", SyntaxTokenKind.Greater);

    syntax.Define("(", SyntaxTokenKind.LeftParen);
    syntax.Define(")", SyntaxTokenKind.RightParen);
    syntax.Define("[", SyntaxTokenKind.LeftSquare);
    syntax.Define("]", SyntaxTokenKind.RightSquare);
    syntax.Define("{", SyntaxTokenKind.LeftCurly);
    syntax.Define("}", SyntaxTokenKind.RightCurly);

    return syntax;
}
