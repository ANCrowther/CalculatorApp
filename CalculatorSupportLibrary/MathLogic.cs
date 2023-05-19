namespace CalculatorSupportLibrary;
public static class MathLogic {
    /// <summary>
    /// Checks for the operators and closed parenthesis as the first input to formula.
    /// </summary>
    /// <returns>Boolean</returns>
    public static bool IsOperator(string input) {
        List<char> operators = new List<char>() { ')', '*', '/', '+', '^', '-' };
        bool output = false;

        foreach (char item in operators) {
            if (input.StartsWith(item)) {
                output = true;
            }
        }
        return output;
    }

    /// <summary>
    /// Calls method to change +/- sign.
    /// </summary>
    /// <returns>Returns the new Entry string.</returns>
    public static string ChangeSign(string input) {
        return SignChange(input);
    }

    /// <summary>
    /// Handles the +/- command logic.
    /// </summary>
    /// <returns>Returns the new Entry string.</returns>
    private static string SignChange(string input) {
        List<string> digits = input.Tokenize();

        if (digits.Count == 0) {
            return "";
        }

        if (digits.Count == 1 && digits[0] == "0") {
            return string.Join("", digits.ToArray());
        }

        if (digits.Last() == ")") {
            return MathLogic.ChangeParenthesisSign(digits);
        }

        if (!IsDigitCheck(digits.Last())) {
            return string.Join("", digits.ToArray());
        }

        return MathLogic.ChangeDigitSign(digits);
    }

    /// <summary>
    /// Takes the most recent digit in the formula and changes its +/-.
    /// </summary>
    /// <param name="digits">tokenized list of formula</param>
    /// <returns>Returns the new Entry string.</returns>
    private static string ChangeDigitSign(List<string> digits) {
        if (digits.Count == 1 && IsDigitCheck(digits[0])) {
            digits.Insert(0, "-");
            return string.Join("", digits.ToArray());
        }

        if (IsDigitCheck(digits.Last())) {
            int i = digits.Count - 1;

            while (i > 0 && IsDigitCheck(digits[i])) {
                i--;
            }

            if (i == 1 && IsDigitCheck(digits[i])) { // 3 => -3
                digits.Insert(0, "-");
            } else if (digits[i] == "(" && IsDigitCheck(digits[i + 1])) { // (3 => (-3
                digits.Insert(i + 1, "-");
            } else if (i > 0 && digits[i] == "-" && digits[i - 1] == "(") { // (-3 => (3
                digits.RemoveAt(i);
            } else if (i == 0 && digits[0] == "-") { // -3 => 3
                digits.RemoveAt(0);
            } else if (i > 0 && IsDigitCheck(digits[i - 1])) { // 4+3 => 4+-3
                digits.Insert(i + 1, "-");
            } else if (i > 2 && digits[i - 1] == ")") { // (2+3)-3 => (2+3)--3
                digits.Insert(i + 1, "-");
            } else if (i > 1 && digits[i] == "-" && !IsDigitCheck(digits[i - 1])) { // 4+-3 => 4+3
                digits.RemoveAt(i);
            }
        }

        return string.Join("", digits.ToArray());
    }

    /// <summary>
    /// Takes the most recent closed parenthesis in the formula and changes its +/-.
    /// </summary>
    /// <param name="digits">tokenized list of formula</param>
    /// <returns>Returns the new Entry string.</returns>
    private static string ChangeParenthesisSign(List<string> digits) {
        int i = digits.Count - 1;
        int parenthesisCount = 0;

        while (i > 0) { // Places the index at the matching open parenthesis based on most recent closed parenthesis group.
            if (digits[i] == ")") {
                parenthesisCount++;
            }
            if (parenthesisCount > 0 && digits[i] == "(") {
                parenthesisCount--;
            }
            if (parenthesisCount == 0) {
                break;
            }
            i--;
        }

        if (i == 0 && digits[i] == "(") { // (32+10) => -(32+10)
            digits.Insert(0, "-");
        } else if (i == 1 && digits[i - 1] == "-") { // -(32+10) => (32+10)
            digits.RemoveAt(0);
        } else if (i > 1 && digits[i - 1].IsArithmeticOperator() && digits[i - 2].IsArithmeticOperator()) {// 42+-(32+10) => 42+(32+10)
            digits.RemoveAt(i - 1);
        } else if (i > 1 && digits[i - 1].IsArithmeticOperator()) { // 42+(32+10) => 42+-(32+10)
            digits.Insert(i, "-");
        } else if (i > 2 && digits[i - 2] == ")") { // (3+2)+(32+10) => (3+2)+-(32+10)
            digits.Insert(i = 1, "-");
        }

        return string.Join("", digits.ToArray());
    }

    /// <summary>
    /// Verifies if the input is a number.
    /// </summary>
    /// <param name="input">String input</param>
    /// <returns>Boolean</returns>
    private static bool IsDigitCheck(string input) {
        return (int.TryParse(input, out _) || double.TryParse(input, out _) || decimal.TryParse(input, out _));
    }
}
