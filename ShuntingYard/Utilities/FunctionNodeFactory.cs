using ShuntingYardLibrary.Nodes;

namespace ShuntingYardLibrary.Utilities;
public static class FunctionNodeFactory {
    public static FunctionNode MakeNode(string inputString) => inputString switch {
        "sin" => new SineNode(),
        "cos" => new CosineNode(),
        "tan" => new TangentNode(),
        _ => null
    };
}
