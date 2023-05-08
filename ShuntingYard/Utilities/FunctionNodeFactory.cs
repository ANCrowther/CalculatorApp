using ShuntingYardLibrary.Nodes;

namespace ShuntingYardLibrary.Utilities;
public static class FunctionNodeFactory {
    public static FunctionNode MakeNode(string inputString) => inputString switch {
        "sin" => new SinNode(),
        "cos" => new CosNode(),
        "tan" => new TanNode(),
        "sec" => new SecNode(),
        "csc" => new CscNode(),
        "cot" => new CotNode(),

        "sinh" => new SinhNode(),
        "cosh" => new CoshNode(),
        "tanh" => new TanhNode(),
        "sech" => new SechNode(),
        "csch" => new CschNode(),
        "coth" => new CothNode(),

        "asin" => new AsinNode(),
        "acos" => new AcosNode(),
        "atan" => new AtanNode(),
        "asec" => new AsecNode(),
        "acsc" => new AcscNode(),
        "acot" => new AcotNode(),

        "asinh" => new AsinhNode(),
        "acosh" => new AcoshNode(),
        "atanh" => new AtanhNode(),
        "asech" => new AsechNode(),
        "acsch" => new AcschNode(),
        "acoth" => new AcothNode(),
        _ => null
    };
}
