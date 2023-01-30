namespace Nyx.TypeSystem;

// public class Scope : ReadOnlyScope
// {
//     public Scope() { }

//     public bool TryDeclare(Symbol symbol) => _scope.TryAdd(symbol.name, symbol);
// }

// public class Namespace : Symbol
// {
//     public override Scope members { get; }

//     public Namespace(string name) : base(name) 
//     {
//         members = new Scope();
//     }
// }

// public class Symbol : Symbol
// {
//     public override Scope members { get; }

//     public Symbol(string name) : base(name) 
//     {
//         members = new Scope();
//     }
// }

// public class Function : Symbol
// {
//     public override Scope? parent { get; }

//     public Function(string name, Scope parent) : base(name)
//     { 
//         this.parent = parent; 
//     }
// }

// public class Variable : Symbol
// {
//     public override Scope? parent { get; }

//     public Variable(string name, Scope? parent) : base(name)
//     { 
//         this.parent = parent; 
//     }
// }