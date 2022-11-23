using CodeAnalysis;

var running = true;
var syntax = SyntaxDefinition.Default();

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

    var lexicalAnalyzer = new LexicalAnalyzer(syntax, input);
    var tokens = lexicalAnalyzer.GetAll().ToList();

    var postlexicalAnalyzer = new PostlexicalAnalyzer(syntax, tokens);
    tokens = postlexicalAnalyzer.GetAll().ToList();

    var syntaxAnaylzer = new SyntaxAnalyzer(syntax, tokens);
    var compilationUnit = syntaxAnaylzer.GetAll();
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