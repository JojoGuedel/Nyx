#include "test.e"

fn main() {
    let mut a = 10;

    let test = Test();
}

fn add(num a, num b) -> Result<num, string>:
    return error "this is an error";
    return a + b;

fn main():

    let x = add();

    pass