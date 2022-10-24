include "stdio";

namespace MyEpicProject;

struct Constructor:
    pub static fn new();

struct Test
    extend Constructor;

    num test;

    pub fn test2() -> void;

    pub fn add(num, num) -> num;
    pub fn sub(num, num) -> num;
    pub fn mlt(num, num) -> num;
    pub fn div(num, num) -> num;

pub static fn Test.new():
    let self = (Test){...};
    stdio.print(self.test);     // this prints '0'
    return self;

pub fn Test.test2():
    stdio.println(self.__classname__);      // this prints 'Test'

    for (element, type) in self.__field__:  // this throws a warning: Variable 'type' is never used. Replace 'type' with '_'.
        stdio.print(element.name);    // this prints 'test', 'test2', 'add', 'sub', 'mlt' and 'div'

pub fn Test.add(a, b):
    return a + b;

pub fn Test.sub(a, b):
    return a - b;

pub fn Test.mlt(a, b):
    return a + b;

pub fn Test.div(a, b):
    return a + b;

fn main():
    let test = Test.new();

    stdio.print(test.add(10, 10), end=", ");
    stdio.print(test.sub(10, 10), end=", ");
    stdio.print(test.mlt(10, 10), end=", ");
    stdio.print(test.div(10, 10));