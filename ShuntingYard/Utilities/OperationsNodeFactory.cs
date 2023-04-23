using ShuntingYardLibrary.Nodes;

namespace ShuntingYardLibrary.Utilities;
public static class OperationsNodeFactory
{
    /// <summary>
    /// Factory that creates the specific operator node for compiling.
    /// </summary>
    /// <param name="inputString">String operator to be converted.</param>
    /// <returns>Operator node.</returns>
    public static OperatorNode MakeNode(string inputString) => inputString switch {
        "+" => new AdditionNode(),
        "-" => new SubtractionNode(),
        "*" => new MultiplicationNode(),
        "/" => new DivisionNode(),
        "^" => new ExponentNode(),
        _ => null,
    };
}
