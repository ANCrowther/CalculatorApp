using CalculatorMauiGUI.Utilities;
using ShuntingYardLibrary;
using System.ComponentModel;
using System.Windows.Input;

namespace CalculatorMauiGUI.ViewModels;

internal partial class CalculatorViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    private string entry = "0";
    private string answer = "";

    private bool isSecond = false;
    private bool isHyp = false;
    private bool isTrig = false;

    private decimal PI = 3.1415926535897932384626433832795m;
    private decimal E = 2.7182818284590452353602874713526m;

    public ICommand AnswerCommand { get; private set; }
    public ICommand BackspaceCommand { get; private set; }
    public ICommand ClearCommand { get; private set; }
    public ICommand DigitCommand { get; private set; }
    public ICommand NegativeDigitCommand { get; private set; }
    public ICommand SaveAnswerCommand { get; private set; }

    public ICommand SecondCommand { get; private set; }
    public ICommand HypCommand { get; private set; }
    public ICommand TrigCommand { get; private set; }

    public bool IsSecond { 
        get {
            return isSecond;
        }
        set {
            isSecond = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSecond"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigSecond"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigHypSecond"));
        }
    }

    public bool IsHyp {
        get {
            return isHyp;
        }
        set {
            isHyp = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsHyp"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigHyp"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigHypSecond"));
        }
    }

    public bool IsTrig {
        get {
            return isTrig;
        }
        set {
            isTrig = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTrig"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigSecond"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigHyp"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrigHypSecond"));
        }
    }

    public bool TrigSecond {
        get {
            return isTrig && isSecond;
        }
    }

    public bool TrigHyp {
        get {
            return isTrig && isHyp;
        }
    }

    public bool TrigHypSecond {
        get {
            return isTrig && isHyp && isSecond;
        }
    }

    public string Entry {
        private set {
            if (entry != value) {
                entry = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Entry"));
            }
        }
        get {
            return entry;
        }
    }

    public string Answer {
        private set {
            if (answer != value) {
                answer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Answer"));
            }
        }
        get {
            if (!answer.IsDecimalInRecentNumber()) {
                return answer;
            } else if (answer.Length < 18) {
                return answer;
            } else {
                return ConvertToScientificNotation(answer);
            }
        }
    }

    public CalculatorViewModel() {
        SecondCommand = new Command(
            execute: () => {
                IsSecond = !IsSecond;
                RefreshCanExecutes();
            });

        HypCommand = new Command(
            execute: () => {
                IsHyp = !IsHyp;
                RefreshCanExecutes();
            });

        TrigCommand = new Command(
            execute: () => {
                IsTrig = !IsTrig;
                RefreshCanExecutes();
            });

        AnswerCommand = new Command(
            execute: () => {
                Answer = ExpressionTree.Evaluate(Entry);
                Entry = "0";
                RefreshCanExecutes();
            },
            canExecute: () => {
                return Entry != "0";
            });

        BackspaceCommand = new Command(
            execute: () => {
                backspace();
                RefreshCanExecutes();
            },
            canExecute: () => {
                return Entry.Length > 1 || Entry != "0";
            });

        ClearCommand = new Command(
            execute: () => {
                Entry = "0";
                Answer = "0";
                RefreshCanExecutes();
            });

        DigitCommand = new Command<string>(
            execute: (string arg) => {
                Entry += (arg == ".") ? CheckDigitsBeforeDecimal() : "";
                if (arg == "PI") {
                    Entry += PI.ToString();
                } else if (arg == "e") {
                    Entry += E.ToString();
                } else {
                    Entry += arg;
                }
                

                if (Entry.StartsWith("0") && !Entry.StartsWith("0.")) {
                    Entry = Entry.Substring(1);
                }
                //Prevent user adding a '(' after a digit.
                if (Entry.Length > 1 && Entry.EndsWith("(") && Char.IsDigit(Entry[Entry.Length - 2])) {
                    backspace();
                }
                //Prevent user adding arithmetic operators immediately after an '(' or another arithmetic operator.
                if (Entry.Length > 1 && (Entry[Entry.Length - 2].IsArithmeticOperator() || Entry[Entry.Length - 2] == '(') && Entry[Entry.Length - 1].IsArithmeticOperator()) {
                    backspace();
                }
                // This check prevents the user from starting formula with anything other than a number or '('.
                if (IsOperator() && Entry[0] != '-' && arg != "(") {
                    Entry = "0";
                }
                RefreshCanExecutes();
            },
            canExecute: (string arg) => {
                return !(arg == "." && entry.IsDecimalInRecentNumber());
            });

        NegativeDigitCommand = new Command(
            execute: () => {
                Entry = ChangeDigitSign();
                RefreshCanExecutes();
            },
            canExecute: () => {
                return Entry != "0";
            });

        SaveAnswerCommand = new Command(
            execute: () => {
                Entry = Answer;
                Answer = "0";
                RefreshCanExecutes();
            },
            canExecute: () => {
                return Entry == "0";
            });
    }

    private void RefreshCanExecutes() {
        ((Command)AnswerCommand).ChangeCanExecute();
        ((Command)BackspaceCommand).ChangeCanExecute();
        ((Command)DigitCommand).ChangeCanExecute();
        ((Command)NegativeDigitCommand).ChangeCanExecute();
        ((Command)SaveAnswerCommand).ChangeCanExecute();
    }

    /// <summary>
    /// Adds a '0' before a leading decimal if no number exists already.
    /// </summary>
    /// <returns>Either "" or "0"</returns>
    private string CheckDigitsBeforeDecimal() {
        return (char.IsDigit(Entry.Last())) ? "" : "0";
    }

    /// <summary>
    /// Converts the decimal answer to scientific notation if the answer is greater than 18 characters.
    /// </summary>
    /// <param name="input">Answer</param>
    /// <returns>Decimal answer in scientific notation format.</returns>
    private string ConvertToScientificNotation(string input) {
        decimal nums = Convert.ToDecimal(input);
        return string.Format("{0:#.######E+00}", nums);
    }

    /// <summary>
    /// Removes the last element in the string.
    /// </summary>
    private void backspace() {
        int index = Entry.NewIndex();
        Entry = Entry.Substring(0, index);
        if (Entry == "") {
            Entry = "0";
        }
    }

    /// <summary>
    /// Handles the +/- command logic.
    /// </summary>
    /// <returns>Returns the new Entry string.</returns>
    private string ChangeDigitSign() {
        List<string> digits = Entry.Tokenize();

        if (digits.Count == 0) {
            return "";
        }

        if (digits.Last() == ")") {
            return ChangeParenthesisSign(digits);
        }

        if (!IsDigitCheck(digits.Last())) {
            return string.Join("", digits.ToArray());
        }

        return ChangeDigitSign(digits);
    }

    /// <summary>
    /// Takes the most recent digit in the formula and changes its +/-.
    /// </summary>
    /// <param name="digits">tokenized list of formula</param>
    /// <returns>Returns the new Entry string.</returns>
    private string ChangeDigitSign(List<string> digits) {
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
    /// Takes the most recent closed parenthesis in the formula and changes its +/-.
    /// </summary>
    /// <param name="digits">tokenized list of formula</param>
    /// <returns>Returns the new Entry string.</returns>
    private string ChangeParenthesisSign(List<string> digits) {
        int i = digits.Count - 1;
        int parenthesisCount = 0;
        
        while (i > 0) { // Places the index at the outermost parenthesis based on most recent closed parenthesis group.
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
        } else if (i > 1 && digits[i-1].IsArithmeticOperator()) { // 42+(32+10) => 42+-(32+10)
            digits.Insert(i, "-");
        } else if (i > 2 && digits[i-2] ==")") { // (3+2)+(32+10) => (3+2)+-(32+10)
            digits.Insert(i = 1, "-");
        }

        return string.Join("", digits.ToArray());
    }

    /// <summary>
    /// Verifies if the input is a number.
    /// </summary>
    /// <param name="input">String input</param>
    /// <returns>Boolean</returns>
    private bool IsDigitCheck(string input) {
        return (int.TryParse(input, out _) || double.TryParse(input, out _) || decimal.TryParse(input, out _));
    }

    /// <summary>
    /// Checks for the operators and closed parenthesis as the first input to formula.
    /// </summary>
    /// <returns>Boolean</returns>
    private bool IsOperator() {
        List<char> operators = new List<char>() { ')', '*', '/', '+', '^', '-' };
        bool output = false;

        foreach (char item in operators) {
            if (Entry.StartsWith(item)) {
                output = true;
            }
        }
        return output;
    }
}
