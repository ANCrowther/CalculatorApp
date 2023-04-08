using ShuntingYardLibrary.Nodes;
using ShuntingYardLibrary.Utilities;
using System.Xml.Linq;

namespace ShuntingYardLibrary;

// All the code in this file is included in all platforms.
public static class ExpressionTree {
    public static decimal Evaluate(string inputValue) {
        INode rootNode = CompileTree(PostFix.PostFixTraversal(inputValue));
        try {
            return rootNode.Evaluate();
        } catch {
            //throw new NullReferenceException(ErrorMessages.EmptyTree);
            return 0;
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
