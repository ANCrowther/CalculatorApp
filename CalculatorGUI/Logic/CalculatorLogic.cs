﻿using ShuntingYardLibrary;
using System.Text.RegularExpressions;

namespace CalculatorGUI.Logic;
public static class CalculatorLogic {
    public static string Entry { get; private set; } = "0";
    public static string Answer { get; private set; } = String.Empty;

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
        if (Entry.Length > 1 && (IsArithmeticOperator(Entry[Entry.Length - 2]) || Entry[Entry.Length - 2] == '(') && IsArithmeticOperator(Entry[Entry.Length - 1])) {
            Backspace();
        }
        // This check prevents the user from starting formula with anything other than a number or '('.
        if (IsOperator() && Entry[0] != '-' && input != "(") {
            Entry = "0";
        }
        // Prevents multiple decimals in same number.
        if (input == "." && IsDecimalUsedAlready(Entry)) {
            Backspace();
        }
    }

    /// <summary>
    /// Calls the PlusMinusDigit method to change the sign of the most recent digit.
    /// </summary>
    public static void ChangeDigitSign() {
        Entry = PlusMinusDigit();
    }

    /// <summary>
    /// Removes the last element in the string.
    /// </summary>
    public static void Backspace() {
        Entry = Entry.Substring(0, Entry.Length - 1);
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
    /// Checks if a decimal is already used in the most current number being entered.
    /// </summary>
    /// <param name="input">Entry</param>
    /// <returns>Boolean</returns>
    private static bool IsDecimalUsedAlready(string input) {
        int count = 0;
        List<char> tempString = new List<char>();
        foreach (char item in input) {
            tempString.Add(item);
        }

        for (int i = tempString.Count - 1; i >= 0; i--) {
            if (tempString[i] == '.') {
                count++;
            }
            //Check ensures method only looks at most recent number inputted.
            if (!(char.IsNumber(tempString[i]) || tempString[i] == '.')) {
                break;
            }
        }
        return (count > 1);
    }

    /// <summary>
    /// Checks for an arithmetic operator.
    /// </summary>
    /// <param name="value">Char value in Entry string</param>
    /// <returns>Boolean</returns>
    private static bool IsArithmeticOperator(char value) {
        return (value == '*' || value == '/' || value == '+' || value == '^' || value == '-');
    }

    /// <summary>
    /// Checks for the operators and closed parenthesis as the first input to formula.
    /// </summary>
    /// <returns>Boolean</returns>
    private static bool IsOperator() {
        List<char> operators = new List<char>() { ')', '*', '/', '+', '^', '-' };
        bool output = false;

        foreach (char item in operators) {
            if (Entry.StartsWith(item)) {
                output = true;
            }
        }
        return output;
    }

    /// <summary>
    /// Adds a '0' before a leading decimal if no number exists already.
    /// </summary>
    /// <returns>Either "" or "0"</returns>
    private static string CheckDigitsBeforeDecimal() {
        return (char.IsDigit(Entry.Last())) ? "" : "0";
    }

    /// <summary>
    /// Takes the most recent digit in the formula and changes its +/-.
    /// </summary>
    /// <returns>Returns the new Entry string.</returns>
    private static string PlusMinusDigit() {
        List<string> digits = Tokenize(Entry);

        if (digits.Count == 0) {
            return "";
        }

        if (!IsDigitCheck(digits.Last())) {
            return string.Join("", digits.ToArray());
        }

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
    /// Verifies if the input is a number.
    /// </summary>
    /// <param name="input">String input</param>
    /// <returns>Boolean</returns>
    private static bool IsDigitCheck(string input) {
        return (int.TryParse(input, out _) || double.TryParse(input, out _) || decimal.TryParse(input, out _));
    }

    /// <summary>
    /// Converts Entry into a list of tokens for easier manipulation by ChangeDigitSign() method. 
    /// </summary>
    /// <param name="inputString">Entry</param>
    /// <returns>Regex list.</returns>
    private static List<string> Tokenize(string inputString) {
        string @pattern = @"[\d]+\.?[\d]*|[-/\+\*\(\)\^]";
        Regex rgx = new Regex(@pattern);
        MatchCollection matches = Regex.Matches(inputString, @pattern);

        return matches.Cast<Match>().Select(match => match.Value).ToList();
    }
}
