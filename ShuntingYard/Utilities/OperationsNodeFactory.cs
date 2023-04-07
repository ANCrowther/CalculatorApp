using ShuntingYardLibrary.Nodes;

namespace ShuntingYardLibrary.Utilities;
public static class OperationsNodeFactory
{
    public static OperatorNode MakeNode(string inputString) => inputString switch {
        "+" => new AdditionNode(),
        "-" => new SubtractionNode(),
        "*" => new MultiplicationNode(),
        "/" => new DivisionNode(),
        "^" => new ExponentNode(),
        _ => null,
    };
}
