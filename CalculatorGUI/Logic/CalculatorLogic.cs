using ShuntingYardLibrary;
using CalculatorSupportLibrary;

namespace CalculatorGUI.Logic;
public static class CalculatorLogic {
    public static string Entry { get; private set; } = "0";
    public static string Answer { get; private set; } = String.Empty;

    public static decimal PI => MathValues.PI;
    public static decimal E => MathValues.E;

    /// <summary>
    /// When equals button is pushed, the formula is run through the ExpressionTree shunting yard algorithm
    /// and assigns the result to the Answer property.
    /// </summary>
    public static void GetAnswer() {
        Answer = ExpressionTree.Evaluate(Entry);
        Entry = "0";
    }

    /// <summary>
    /// Adds the digit/operator button inputs to the entry property to build the formula string.
    /// Conducts checks to prevent user from inputting bad arithmetic.
    /// </summary>
    /// <param name="input">Representative button.</param>
    public static void DigitCommand(string input) {
        Entry += (input == ".") ? CheckDigitsBeforeDecimal() : "";
        Entry += input;

        if (Entry.StartsWith("0") && !Entry.StartsWith("0.")) {
            Entry = Entry.Substring(1);
        }
        // Prevent user adding a '(' after a digit.
        if (Entry.Length > 1 && Entry.EndsWith("(") && Char.IsDigit(Entry[Entry.Length - 2])) {
            Backspace();
        }
        // Prevent user adding arithmetic operators immediately after an '(' or another arithmetic operator.
        if (Entry.Length > 1 && (Entry[Entry.Length - 2].IsArithmeticOperator() || Entry[Entry.Length - 2] == '(') && Entry[Entry.Length - 1].IsArithmeticOperator()) {
            Backspace();
        }
        // This check prevents the user from starting formula with anything other than a number or '('.
        if (MathLogic.IsOperator(Entry) && Entry[0] != '-' && input != "(") {
            Entry = "0";
        }
        // Prevents multiple decimals in same number.
        if (input == "." && Entry.IsDecimalInRecentNumber()) {
            Backspace();
        }
    }


    /// <summary>
    /// Calls the PlusMinusDigit method to change the sign of the most recent digit.
    /// </summary>
    public static void ChangeDigitSign() {
        Entry = MathLogic.ChangeSign(Entry);
    }

    /// <summary>
    /// Removes the last element in the string.
    /// </summary>
    public static void Backspace() {
        int index = Entry.NewIndex();
        Entry = Entry.Substring(0, index);
        if (Entry == "") {
            Entry = "0";
        }
    }

    /// <summary>
    /// Saves the answer to the entry property and clears the answer property. Let's the user use the previous answer
    /// in the new formula.
    /// </summary>
    public static void SaveAnswer() {
        Entry = Answer;
        Answer = String.Empty;
    }

    /// <summary>
    /// Clears the entry and answer properties.
    /// </summary>
    public static void ClearInput() {
        Entry = "0";
        Answer = String.Empty;
    }

    /// <summary>
    /// Adds a '0' before a leading decimal if no number exists already.
    /// </summary>
    /// <returns>Either "" or "0"</returns>
    private static string CheckDigitsBeforeDecimal() {
        return (char.IsDigit(Entry.Last())) ? "" : "0";
    }
}
