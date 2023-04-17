using ShuntingYardLibrary.Nodes;
using ShuntingYardLibrary.Utilities;
using System.Xml.Linq;

namespace ShuntingYardLibrary;

// All the code in this file is included in all platforms.
public static class ExpressionTree {
    public static string Evaluate(string inputValue) {
        INode rootNode;
        try {
            rootNode = CompileTree(PostFix.PostFixTraversal(inputValue));
        } catch (ArgumentException) {
            return ErrorMessages.Mismatch;
        }

        try {
            return rootNode.Evaluate().ToString();
        } catch (DivideByZeroException) {
            return ErrorMessages.DivideByZero;
        } catch (ArgumentException) {
            return ErrorMessages.Mismatch;
        } catch (Exception ex) {
            return "Error";
        }
    }

    private static INode CompileTree(string inputString) {
        var tokenList = TokenManager.Tokenize(inputString);
        Stack<INode> nodeStack = new Stack<INode>();

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
