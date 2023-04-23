using System.Text.RegularExpressions;

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
    /// Converts the infix string formula to postfix.
    /// </summary>
    /// <param name="inputString">String infix formula.</param>
    /// <returns>String postfix formula.</returns>
    /// <exception cref="ArgumentException">Mismatching parenthesis exception.</exception>
    /// <exception cref="FormatException">Invalid token exception.</exception>
    public static string Traversal(string inputString) {
        int parenthesisCount = 0;
        var list = Infix.Traversal(inputString);

        Stack<char> operatorStack = new Stack<char>();
        Queue<string> numberQueue = new Queue<string>(list.Capacity);

        foreach (string token in list) {
            if (int.TryParse(token, out _) ||
                double.TryParse(token, out _) ||
                decimal.TryParse(token, out _) ||
                Regex.Match(token, @"[A-Za-z]+[0-9]+").Success) {
                numberQueue.Enqueue(token);
            } else {
                if (operators.Contains(token[0])) {
                    while (operatorStack.Count != 0 &&
                          operatorPrecedence[operatorStack.Peek()] > operatorPrecedence[token[0]]) {
                        numberQueue.Enqueue(operatorStack.Pop().ToString());
                    }
                    operatorStack.Push(token[0]);
                } else if (token.StartsWith("(")) {
                    parenthesisCount++;
                    operatorStack.Push(token[0]);
                } else if (token.StartsWith(")")) {
                    parenthesisCount--;
                    try {
                        while (operatorStack.Peek() != '(') {
                            numberQueue.Enqueue(operatorStack.Pop().ToString());
                        }
                    } catch (Exception) {
                        throw new ArgumentException(ErrorMessages.Mismatch);
                    }
                } else {
                    throw new FormatException(string.Format(ErrorMessages.InvalidToken, token));
                }
            }
        }

        while (operatorStack.Count > 0) {
            if (operatorStack.Peek() != '(' || operatorStack.Peek() != ')') {
                numberQueue.Enqueue(operatorStack.Pop().ToString());
            }
        }

        if (parenthesisCount != 0) {
            throw new ArgumentException(ErrorMessages.Mismatch);
        }

        return String.Join(" ", numberQueue.ToArray());
    }
}
