using ShuntingYardLibrary;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CalculatorMauiGUI.ViewModels
{
    internal partial class CalculatorViewModel : INotifyPropertyChanged {
        private string entry = "0";
        private string answer = "";

        public event PropertyChangedEventHandler PropertyChanged;

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
                return answer;
            }
        }

        public CalculatorViewModel()
        {
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
                    if (Entry.Length > 1 && Entry.EndsWith("(") && Char.IsDigit(Entry[Entry.Length - 2])) {
                        backspace();
                    }
                    if (Entry.Length > 1 && (IsArithmeticOperator(Entry[Entry.Length - 2]) || Entry[Entry.Length - 2] == '(') && IsArithmeticOperator(Entry[Entry.Length - 1])) {
                        backspace();
                    }

                    RefreshCanExecutes();
                    if (IsOperator()) {
                        if (arg != "(" && !Entry.StartsWith("(")) {
                            Entry = "0";
                        }
                        RefreshCanExecutes();
                    }
                },
                canExecute: (string arg) => {
                    return !(arg == "." && !NoDecimalUsedInNumber());
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

        private string CheckDigitsBeforeDecimal() {
            return (char.IsDigit(Entry.Last())) ? "" : "0";
        }

        private void backspace() {
            Entry = Entry.Substring(0, Entry.Length - 1);
            if (Entry == "") {
                Entry = "0";
            }
        }

        private bool NoDecimalUsedInNumber() {
            bool notUsed = true;
            List<char> tempString = new List<char>();
            foreach (char item in Entry) {
                tempString.Add(item);
            }

            for (int i = tempString.Count - 1; i >= 0; i--) {
                if (tempString[i] == '.') {
                    notUsed = false;
                    break;
                }
                if (!char.IsNumber(tempString[i])) {
                    break;
                }
            }
            return notUsed;
        }

        private string ChangeDigitSign() {
            string output = "";
            List<string> digits = Tokenize(Entry);

            if (digits.Count == 1 && IsDigitCheck(digits[0])) {
                digits.Add(")");
                digits.Insert(0, "-");
                digits.Insert(0, "(");
                return RecombineString(digits);
            }

            bool isDigit = IsDigitCheck(digits.Last());

            if (digits.Count > 3) {
                int index = digits.Count - 1;

                if (digits[index] == ")" && digits[index - 2] == "-" && digits[index - 3] == "(") {
                    digits.RemoveAt(index);
                    digits.RemoveAt(index - 2);
                    digits.RemoveAt(index - 3);
                    output = RecombineString(digits);
                }
            }

            if (isDigit) {
                int index = digits.Count - 1;

                if (index == 1 && digits[index - 1] == "(") {
                    digits.Add(")");
                    digits.Insert(index, "-");
                    output = RecombineString(digits);
                } else if (index > 2 && digits[index - 1] == "-" && digits[index - 2] == "(") {
                    digits.RemoveAt(index - 1);
                    output = RecombineString(digits);
                } else if (index > 1 && !IsDigitCheck(digits[index - 1])) {
                    digits.Add(")");
                    digits.Insert(index, "-");
                    digits.Insert(index, "(");
                    output = RecombineString(digits);
                }
            }

            return output;
        }

        private string RecombineString(List<string> list) {
            string output = "";
            foreach (string item in list) {
                output += item;
            }
            return output;
        }

        private bool IsDigitCheck(string input) {
            return (int.TryParse(input, out _) || double.TryParse(input, out _) || decimal.TryParse(input, out _));
        }

        public List<string> Tokenize(string inputString) {
            string @pattern = @"[\d]+\.?[\d]*|[-/\+\*\(\)\^]";
            Regex rgx = new Regex(@pattern);
            MatchCollection matches = Regex.Matches(inputString, @pattern);

            return matches.Cast<Match>().Select(match => match.Value).ToList();
        }

        private bool IsOperator(string value) {
            return (value == ")" || value == "*" || value == "/" || value == "+" || value == "^" || value == "-");
        }

        private bool IsArithmeticOperator(char value) {
            return (value == '*' || value == '/' || value == '+' || value == '^' || value == '-');
        }

        private bool IsOperator() {
            List<char> operators = new List<char>() { '(', ')', '*', '/', '+', '^' };
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
