include "stdio.h" as stdio;

section std;

struct Enumerator<T>:
    pub def get_current() -> T;
    pub def move_next() -> bool;
    pub def reset() -> null;

pub constructor Enumerator(
        get_current: def() -> T,
        move_next: def() -> T, 
        reset: def() -> T
    ) -> null:

    self.get_current = get_current;
    self.move_next = move_next;
    self.reset = reset;

interface IEnumerator<T>:
    pub abstract def get_Enumerator() -> Enumerator<T>;


struct string:
    extends IEnumerator<char>;

    var _val: char[];
    pub var length: uint;

    pub operator [] (int index) -> char;

    section enumerator:
        var _enu: Enumerator<char>;
        mut var _index: int;

pub constructor string() -> null:
    def get_current() -> char:
        return self._val[self.enumerator._index];
    
    def move_next() -> bool:
        return ++self.enumerator._index < self.length;

    def reset() -> null:
        self.enumerator._index = 0;

    self.enumerator._enu = new Enumerator(
        get_current;
        move_next;
        reset;
    );

pub operator string[] (int index) -> char:
    return self._val[index];

pub def string.get_enumerator() -> Enumerator<char>:
    return self.enumerator._enu;

pub static def main() -> null:
    var str: string = "test";
    
    for var c in str:
        stdio.printf(c);