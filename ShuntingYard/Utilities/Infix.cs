using ShuntingYardLibrary.Nodes;

namespace ShuntingYardLibrary.Utilities;
public static class Infix {
    /// <summary>
    /// Takes the input string from the GUI, checks for negative numbers '(-x)' and adds a '0' => '(0-x)'
    ///  to allow the shunting yard to work with negative numbers.
    /// </summary>
    /// <param name="input">Entry from the GUI.</param>
    /// <returns>Token list of the input.</returns>
    public static List<INode> Compile(string input) {
        input = input.Replace(" ", string.Empty);
        List<string> list = input.Tokenize();
        List<INode> output = new();

        for (int i = 0; i < list.Count; i++) {
            if (i == 0 && list[i] == "-") {
                output.Add(NodeGenerator.MakeNode("-1"));
                output.Add(NodeGenerator.MakeNode("*"));
            } else if (list[i] == "-" && IsNotNumber(list[i - 1])) {
                output.Add(NodeGenerator.MakeNode("-1"));
                output.Add(NodeGenerator.MakeNode("*"));
            } else {
                output.Add(NodeGenerator.MakeNode(list[i]));
            }
        }

        return output;
    }

    /// <summary>
    /// Checks if input is a number.
    /// </summary>
    /// <param name="value">String input of current token.</param>
    /// <returns>Boolean.</returns>
    private static bool IsNotNumber(string value) {
        return !(int.TryParse(value, out _) || double.TryParse(value, out _) || decimal.TryParse(value, out _));
    }
}
