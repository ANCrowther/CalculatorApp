using ShuntingYardLibrary.Nodes;
using ShuntingYardLibrary.Utilities;

namespace ShuntingYardLibrary;

public static class ExpressionTree {
    /// <summary>
    /// Evaluates the Entry string from GUI.
    /// </summary>
    /// <param name="inputValue">String formula from GUI.</param>
    /// <returns>String answer to formula.</returns>
    public static string Evaluate(string inputValue) {
        INode rootNode;

        try {
            rootNode = Compile(PostFix.Compile(inputValue));
        } catch (ArgumentException) {
            return ErrorMessages.Mismatch;
        }

        try {
            return rootNode.Evaluate().ToString();
        } catch (DivideByZeroException) {
            return ErrorMessages.DivideByZero;
        } catch (Exception ex) {
            return $"ERR: {ex.Message}";
        }
    }

    /// <summary>
    /// Creates a binary tree to solve the formula.
    /// </summary>
    /// <param name="inputs">List of PostFix nodes.</param>
    /// <returns>Remaining node holding the answer.</returns>
    private static INode Compile(List<INode> inputs) {
        var tokens = inputs;
        Stack<INode> nodeStack = new();

        foreach (INode token in tokens) { 
            switch(token) {
                case FunctionNode trigNode:
                    INode xNode = nodeStack.Pop();
                    trigNode.XNode = xNode;
                    nodeStack.Push(trigNode);
                    break;
                case OperatorNode opNode:
                    INode right = nodeStack.Pop();
                    INode left = nodeStack.Pop();
                    opNode.RightNode = right;
                    opNode.LeftNode = left;
                    nodeStack.Push(opNode);
                    break;
                case NumberNode _:
                    nodeStack.Push(token);
                    break;
                default:
                    break;
            }
        }
        return nodeStack.Pop();
    }
}
