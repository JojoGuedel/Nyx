include "stdio.h" as stdio;

section std;

interface IEnumerator<T>:
    pub def get_current() -> T;
    pub def move_next() -> bool;

struct ArrayEnumeartor<T>:
    extends IEnumeartor<T>;

    var _val: T[];
    var _index uint;

pub constructor ArrayEnumeartor<T>(val: T[]) -> null:
    self._val = val;

pub def ArrayEnumeartor.get_current() -> T:
    return self._val[self._index];

pub def ArrayEnumeartor.move_next() -> bool:
    return ++self._index < self._val.length();

interface IEnumerable<T>:
    pub def get_enumerator() -> IEnumeartor<T>;

struct string:
    extends IEnumerable<char>;

    var _val: char[];

    pub operator [] (int index) -> char;

pub constructor string() -> null:
    self._val = new char[0];

pub constructor string(var val: char[]):
    self._val = val;

pub operator string[] (int index) -> char:
    return self._val[index];

pub def string.get_enumerator() -> IEnumeartor<Char>:
    return new ArrayEnumeartor<char>(self._val);

pub static def main() -> null:
    var str: string = "test";
    
    for var c in str:
        stdio.printf(c);