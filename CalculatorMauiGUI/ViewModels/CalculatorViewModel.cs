using ShuntingYardLibrary;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CalculatorMauiGUI.ViewModels
{
    internal partial class CalculatorViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string entry = "0";
        private string answer = "";

        public ICommand AnswerCommand { private set; get; }
        public ICommand BackspaceCommand { private set; get; }
        public ICommand ClearCommand { private set; get; }
        public ICommand DigitCommand { private set; get; }
        public ICommand NegativeDigitCommand { private set; get; }
        public ICommand SaveAnswerCommand { private set; get; }

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
                if (NoDecimalUsedInNumber(answer)) {
                    return answer;
                } else if (answer.Length < 18) {
                    return answer;
                } else {
                    return ConvertToScientificNotation(answer);
                }
            }
        }

        public CalculatorViewModel() {
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
                    Entry += arg;

                    if (Entry.StartsWith("0") && !Entry.StartsWith("0.")) {
                        Entry = Entry.Substring(1);
                    }
                    //Prevent user adding a '(' after a digit.
                    if (Entry.Length > 1 && Entry.EndsWith("(") && Char.IsDigit(Entry[Entry.Length - 2])) {
                        backspace();
                    }
                    //Prevent user adding arithmetic operators immediately after an '(' or another arithmetic operator.
                    if (Entry.Length > 1 && (IsArithmeticOperator(Entry[Entry.Length - 2]) || Entry[Entry.Length - 2] == '(') && IsArithmeticOperator(Entry[Entry.Length - 1])) {
                        backspace();
                    }
                    // This check prevents the user from starting formula with anything other than a number or '('.
                    if (IsOperator() && arg != "(") {
                        Entry = "0";
                    }
                    RefreshCanExecutes();
                },
                canExecute: (string arg) => {
                    return !(arg == "." && !NoDecimalUsedInNumber(entry));
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
            Entry = Entry.Substring(0, Entry.Length - 1);
            if (Entry == "") {
                Entry = "0";
            }
        }

        /// <summary>
        /// Checks if a decimal is already used in the most current number being entered.
        /// </summary>
        /// <param name="input">Entry</param>
        /// <returns>Boolean</returns>
        private bool NoDecimalUsedInNumber(string input) {
            bool notUsed = true;
            List<char> tempString = new List<char>();
            foreach (char item in input) {
                tempString.Add(item);
            }

            for (int i = tempString.Count - 1; i >= 0; i--) {
                if (tempString[i] == '.') {
                    notUsed = false;
                    break;
                }
                //Check ensures method only looks at most recent number inputted.
                if (!char.IsNumber(tempString[i])) {
                    break;
                }
            }
            return notUsed;
        }

        /// <summary>
        /// Takes the most recent digit in the formula and changes its +/-.
        /// If changing to '-', then adds parenthesis '(-x)'.
        /// If changing to '+', then removes parenthesis and negative sign 'x'.
        /// </summary>
        /// <returns>Returns the new Entry string.</returns>
        private string ChangeDigitSign() {
            List<string> digits = Tokenize(Entry);

            // Handles turning the 1st number into a negative
            if (digits.Count == 1 && IsDigitCheck(digits[0])) {
                digits.Add(")");
                digits.Insert(0, "-");
                digits.Insert(0, "(");
                return string.Join("", digits.ToArray());
            }

            bool isDigit = IsDigitCheck(digits.Last());
            int index = digits.Count - 1;

            //Turns a negative number into positive.
            if (digits.Count > 3) {
                if (digits[index] == ")" && digits[index - 2] == "-" && digits[index - 3] == "(") {
                    digits.RemoveAt(index);
                    digits.RemoveAt(index - 2);
                    digits.RemoveAt(index - 3);
                }
            }

            //Turns the last number in formula into a negative number.
            if (isDigit) {
                if (index == 1 && digits[index - 1] == "(") {
                    digits.Add(")");
                    digits.Insert(index, "-");
                } else if (index > 2 && digits[index - 1] == "-" && digits[index - 2] == "(") {
                    digits.RemoveAt(index - 1);
                } else if (index > 1 && !IsDigitCheck(digits[index - 1])) {
                    digits.Add(")");
                    digits.Insert(index, "-");
                    digits.Insert(index, "(");
                }
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
        /// Converts Entry into a list of tokens for easier manipulation by ChangeDigitSign() method. 
        /// </summary>
        /// <param name="inputString">Entry</param>
        /// <returns>Regex list.</returns>
        public List<string> Tokenize(string inputString) {
            string @pattern = @"[\d]+\.?[\d]*|[-/\+\*\(\)\^]";
            Regex rgx = new Regex(@pattern);
            MatchCollection matches = Regex.Matches(inputString, @pattern);

            return matches.Cast<Match>().Select(match => match.Value).ToList();
        }

        /// <summary>
        /// Checks for an arithmetic operator.
        /// </summary>
        /// <param name="value">Char value in Entry string</param>
        /// <returns>Boolean</returns>
        private bool IsArithmeticOperator(char value) {
            return (value == '*' || value == '/' || value == '+' || value == '^' || value == '-');
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
}
