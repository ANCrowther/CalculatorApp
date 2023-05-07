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
        _ => null
    };
}
