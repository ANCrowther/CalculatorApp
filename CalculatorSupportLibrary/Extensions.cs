using System.Text.RegularExpressions;

namespace CalculatorSupportLibrary;

// All the code in this file is included in all platforms.
public static class Extensions {
    /// <summary>
    /// Verifies the char is a letter.
    /// </summary>
    /// <param name="input">Char to be checked.</param>
    /// <returns>Boolean</returns>
    private static bool IsLetter(this char input) {
        return (((int)input > 96 && (int)input < 123) || ((int)input > 64 && (int)input < 91)) ? true : false;
    }

    /// <summary>
    /// Supports backspace button, looks for index to remove all letters of a function at once.
    /// </summary>
    /// <param name="input">Entry formula string</param>
    /// <returns>Index of either last position, or first letter in function.</returns>
    public static int NewIndex(this string input) {
        int output = input.Length - 1;
        for (int i = output; i > 0; i--) {
            if (input[i].IsLetter()) {
                output--;
            } else {
                break;
            }
        }

        if ((output < input.Length - 1) && !input[output].IsLetter() && input[output + 1].IsLetter()) {
            output++;
        }

        return output;
    }

    /// <summary>
    /// Checks to see if decimal is already used in recent number
    /// </summary>
    /// <param name="input">Entry formula string</param>
    /// <returns>Boolean</returns>
    public static bool IsDecimalInRecentNumber(this string input) {
        bool isUsed = false;
        List<char> tempString = new List<char>();
        foreach (char item in input) {
            tempString.Add(item);
        }

        for (int i = tempString.Count - 1; i >= 0; i--) {
            if (tempString[i] == '.') {
                isUsed = true;
                break;
            }
            //Check ensures method only looks at most recent number inputted.
            if (!char.IsNumber(tempString[i])) {
                break;
            }
        }
        return isUsed;
    }

    /// <summary>
    /// Checks for an arithmetic operator.
    /// </summary>
    /// <param name="value">Char value in Entry string</param>
    /// <returns>Boolean</returns>
    public static bool IsArithmeticOperator(this char value) {
        return (value == '*' || value == '/' || value == '+' || value == '^' || value == '-' || value == '%');
    }

    /// <summary>
    /// Checks for an arithmetic operator.
    /// </summary>
    /// <param name="value">String value in Entry string list</param>
    /// <returns>Boolean</returns>
    public static bool IsArithmeticOperator(this string value) {
        return (value == "*" || value == "/" || value == "+" || value == "^" || value == "-" || value == "%");
    }

    /// <summary>
    /// Converts Entry into a list of tokens for easier manipulation by ChangeDigitSign() method. 
    /// </summary>
    /// <param name="inputString">Entry</param>
    /// <returns>Regex list.</returns>
    public static List<string> Tokenize(this string inputString) {
        string @pattern = @"[\d]+\.?[\d]*|[A-Za-z]+|[-/\+\*\(\)\^\%]";
        Regex rgx = new Regex(@pattern);
        MatchCollection matches = Regex.Matches(inputString, @pattern);

        return matches.Cast<Match>().Select(match => match.Value).ToList();
    }
}
