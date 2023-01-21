# NYX
This is a project of mine, where I try to create a compiler for my own programminglanguage called nyx. I want to create a simple language with similar features like C but with a view additions to add some object oriented programming. Furthermore, I want to make the language memorysafe with the technics used in the Rust programminglanguage.

As for now, the compiler is written in C# but the goal is to make the compiler self-hosted. Furthermore, the goal for the first view iterations of the compiler is to compile to .Net Intermediate Language. Later, I will try to use LLVM to be able to compile to native and interact with code written in C.

At the moment, the compiler isn't yet wroking. But I'm trying to create a first version of the compiler with less features, so I can test and experiment with the language as soon as possible. This will give me the opportunity to make adjustments and changes to the syntax, so that the language feels natural to program in.

## Syntax
The syntax of nyx will be mainly a mixture between C#, and Python. 

Here is an example (main.nyx): 

```
struct StructA:
    var a: i32 => pub get, set;
    var b: i32 => get;

    pub func test(var a: i32): i32 => get;

func StructA.test(a):
    return self.a + a;

static func test(mut var a: i32):
    var b = 10;
    a += b;
    return a;

static var d: i32 = 20;

static func main():
    var a: StructA = StructA();
    
    var b: i32 = test(10);
    var c: i32 = a.test(b);

```
This example demonstrats roughly what the first version of the compiler should be able to compile but the syntax will probably change and get some tweeks here and there.

## Roadmap

- [x] Lexer
- [x] Parser
- [ ] Binder
- [ ] Lowerer
- [ ] Generate IL

## License

Licensed under the MIT license ([LICENSE](LICENSE) or [MIT](https://choosealicense.com/licenses/mit/))