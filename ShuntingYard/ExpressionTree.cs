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
            rootNode = CompileTree(PostFix.Traversal(inputValue));
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
    /// <param name="inputString">Postfix string.</param>
    /// <returns>RootNode holding the answer.</returns>
    private static INode CompileTree(string inputString) {
        var tokenList = TokenManager.Tokenize(inputString);
        Stack<INode> nodeStack = new();

        foreach (string token in tokenList) {
            INode node = NodeGenerator.MakeNode(token);

            switch (node) {
                case OperatorNode opNode:
                    INode right = nodeStack.Pop();
                    INode left = nodeStack.Pop();
                    opNode.RightNode = right;
                    opNode.LeftNode = left;
                    nodeStack.Push(opNode);
                    break;
                case NumberNode _:
                    nodeStack.Push(node);
                    break;
                default:
                    break;
            }
        }

        return nodeStack.Pop();
    }
}
