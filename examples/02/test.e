struct Constructor {
    pub static fn new(...) -> Result<Test, Error>;
}

struct Test {
    extend Constructor;

    mut num a;
    mut num b;
    num c;
}

struct Test:
    extend Constructor;

    mut num a;
    mut num b;
    num c;

pub static fn Test.test -> Result<Test, Error>:
    pass;

pub static fn Test.new(a: num=10, b: num=20) -> Result<Test, Error>{
    self = {
        a: a,
        b: b,
        ...
    }
}