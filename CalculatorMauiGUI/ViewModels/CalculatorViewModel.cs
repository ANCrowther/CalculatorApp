using ShuntingYardLibrary;
using System.ComponentModel;
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
            bool isNumber = true;
            string output = "";
            List<char> tokens = new List<char>();

            foreach (char item in Entry) {
                tokens.Add(item);
            }

            for (int i = tokens.Count; i >= 0; i--) {
                if (isNumber == true && !(char.IsNumber(tokens[i]) || tokens[i] == '.')){
                    isNumber = false;
                }
                if (i > 1) {
                    if (isNumber == false && IsOperator(tokens[i - 1]) && !(tokens[i - 1] == '(')) {
                        tokens.Insert(i, '-');
                        break;
                    }
                    if (tokens[i] == '-' && !IsOperator(tokens[i - 1]) && !(tokens[i-1] == '(')) {
                        tokens.RemoveAt(i);
                        break;
                    }
                }
                if (i == 0 && char.IsNumber(tokens[i])) {
                    tokens.Insert(0, '-');
                    break;
                }
            }

            foreach (char item in tokens) {
                output += item;
            }
            
            return output;
        }

        private bool IsOperator(char value) {
            return (value == ')' || value == '*' || value == '/' || value == '+' || value == '^');
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
