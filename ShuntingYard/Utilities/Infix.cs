namespace ShuntingYardLibrary.Utilities;
public static class Infix {
    /// <summary>
    /// Takes the input string from the GUI, checks for negative numbers '(-x)' and adds a '0' => '(0-x)'
    ///  to allow the shunting yard to work with negative numbers.
    /// </summary>
    /// <param name="input">Entry from the GUI.</param>
    /// <returns>Token list of the input.</returns>
    public static List<string> Traversal(string input) {
        input = input.Replace(" ", string.Empty);
        List<string> list = TokenManager.Tokenize(input);
        List<string> output = new();
        bool isOpenParenthesis = false;

        for (int i = 0; i < list.Count; i++) {
            if (list[i] == "(") {
                isOpenParenthesis = true;
                output.Add(list[i]);
            }else if (isOpenParenthesis == true && list[i] == "-") {
                output.Add("0");
                output.Add(list[i]);
                isOpenParenthesis = false;
            } else if (isOpenParenthesis == true && list[i] != "-") {
                isOpenParenthesis = false;
                output.Add(list[i]);
            } else {
                output.Add(list[i]);
            }
        }
        return output;
    }
}
