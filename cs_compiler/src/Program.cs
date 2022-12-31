using Nyx.Analysis;

var running = true;
var syntax = SyntaxDefinition.Default();
var syntaxNodeWriter = new SytnaxNodeWriter(Console.Out);

var input =
@"
func b(c,):
    while true:
        var a = 10;
        a++;";

Compile(input);
Console.ReadKey(true);

// while (running)
// {
//     Console.Write("> ");

//     var input = Console.ReadLine();
//     input =
// @"
// func b(c,):
//         // var a = 10;
//     if true:
//         pass;
//     else if true:
//         pass;
//     else:
//         pass;

// func d():
//     pass;";

//     if (input is null)
//         input=String.Empty;

//     if (input.Length >= 1 && input[0] == '#')
//     {
//         ManageEscapeCommands(input.Substring(1));
//         continue;
//     }

//     Compile(input);
// }

void Compile(string input)
{
    var lexicalAnalyzer = new LexicalAnalyzer(syntax, input);
    var tokens = lexicalAnalyzer.GetAll().ToList();
    syntaxNodeWriter.Write(tokens);
    Console.WriteLine();

    var postLexicalAnalyzer = new PostlexicalAnalyzer(syntax, tokens);
    tokens = postLexicalAnalyzer.GetAll().ToList();
    syntaxNodeWriter.Write(tokens);
    Console.WriteLine();

    var syntaxAnaylzer = new SyntaxAnalyzer(syntax, tokens);
    var compilationUnit = syntaxAnaylzer.GetAll().ToList();
    syntaxNodeWriter.Write(compilationUnit);
    Console.WriteLine();

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