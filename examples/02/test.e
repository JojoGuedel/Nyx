struct Constructor {
    pub static fn new(...) -> Result<Test, Error>;
}

struct Test {
    extend Constructor;

    mut num a;
    mut num b;
    num c;
}

fn Test.new(a: num=10, b: num=20) -> Result<Test, Error>{
    self = {
        a: a,
        b: b,
        ...
    }
}


fn Test.destory() {
    self.free();
}