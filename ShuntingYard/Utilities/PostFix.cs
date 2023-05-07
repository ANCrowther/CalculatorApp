using ShuntingYardLibrary.Nodes;
using System.ComponentModel.Design;

namespace ShuntingYardLibrary.Utilities;
public static class PostFix {
    static HashSet<char> operators = new HashSet<char>(new char[] { '+', '-', '*', '/', '^' });
    static Dictionary<char, int> operatorPrecedence = new Dictionary<char, int> {
        ['('] = 0,
        ['+'] = 10,
        ['-'] = 10,
        ['*'] = 20,
        ['/'] = 20,
        ['^'] = 50,
        [')'] = 100
    };

    /// <summary>
    /// Converts the infix string formula to postfix list.
    /// </summary>
    /// <param name="input">String infix formula.</param>
    /// <returns>List of nodes in postfix format.</returns>
    /// <exception cref="ArgumentException">Mismatching parenthesis exception.</exception>
    /// <exception cref="FormatException">Invalid token exception.</exception>
    public static List<INode> Compile(string input) {
        List<INode> list = Infix.Compile(input);
        Stack<INode> operatorStack = new();
        Queue<INode> numberQueue = new(list.Capacity);
        int parenthesisCount = 0;

        foreach (INode node in list) {
            if (node is NumberNode) {
                numberQueue.Enqueue(node);
            } else if (node is FunctionNode) {
                operatorStack.Push(node);
            } else{
                if (operators.Contains((node as OperatorNode).Precedence)) {
                    while (operatorStack.Count != 0 && !(operatorStack.Peek() is FunctionNode) && (operatorPrecedence[(operatorStack.Peek() as OperatorNode).Precedence] > operatorPrecedence[(node as OperatorNode).Precedence])) {
                        numberQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Push(node);
                } else if (node is OpenParenthesisNode) {
                    parenthesisCount++;
                    operatorStack.Push(node);
                } else if (node is ClosedParenthesisNode) {
                    parenthesisCount--;
                    try {
                        while (!(operatorStack.Peek() is OpenParenthesisNode)) {
                            numberQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Pop();
                        if (operatorStack.Peek() is FunctionNode) {
                            numberQueue.Enqueue(operatorStack.Pop());
                        }
                    } catch (Exception) {
                        throw new ArgumentException(ErrorMessages.Mismatch);
                    }
                }
            }
        }

        while (operatorStack.Count > 0) {
            if (!(operatorStack.Peek() is OpenParenthesisNode) || !(operatorStack.Peek() is ClosedParenthesisNode)) {
                numberQueue.Enqueue(operatorStack.Pop());
            }
        }

        if (parenthesisCount != 0) {
            throw new ArgumentException(ErrorMessages.Mismatch);
        }

        return numberQueue.ToList();
    }
}
