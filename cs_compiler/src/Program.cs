using Nyx.Analysis;
using Nyx.Diagnostics;
using Nyx.Evaluation;
using Nyx.Utils;

var running = true;
var syntax = SyntaxDefinition.Default();
var syntaxNodeWriter = new SytnaxNodeWriter(Console.Out);

var input =
@"
// no public modifier
// no abstract modifier
// no extend syntax

// no templates

static func test(mut var a: i32):
    var b: i32 = 20;
    a += a = b = c = 10;
    return a;

static func main():
    mut var a: i32 = 20;
    // var b: i32 = test(10);
    // a = test(b);
    var b = a + 10";

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
    var diagnosticWriter = new DiagnosticWriter(Console.Out, input, 2);

    var textInfo = new TextInfo(input);
    Console.WriteLine(textInfo.ToString());

    var lexicalAnalyzer = new LexicalAnalyzer(syntax, input);
    var tokens = lexicalAnalyzer.GetAll().ToList();

    var postLexicalAnalyzer = new PostlexicalAnalyzer(syntax, tokens);
    tokens = postLexicalAnalyzer.GetAll().ToList();

    var syntaxAnaylzer = new SyntaxAnalyzer(syntax, tokens);
    var compilationUnit = syntaxAnaylzer.GetAll().ToList();
    syntaxNodeWriter.Write(compilationUnit);
    Console.WriteLine();

    diagnosticWriter.Write(lexicalAnalyzer.diagnostics);
    diagnosticWriter.Write(postLexicalAnalyzer.diagnostics);
    diagnosticWriter.Write(syntaxAnaylzer.diagnostics);

    var evaluator = new Evaluator(input, compilationUnit[0]);
    evaluator.Evaluate();
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