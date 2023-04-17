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
                    if (Entry.Length > 1 || Entry != "0") {
                    }
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
                    RefreshCanExecutes();
                    if (Entry.StartsWith(")")) {
                        Entry = "0";
                        RefreshCanExecutes();
                    }
                },
                canExecute: (string arg) => {
                    return !(arg == "." && !NoDecimalUsedInNumber());
                });

            SaveAnswerCommand = new Command(
                execute: () => {
                    Entry = Answer;
                    Answer = "0";
                    RefreshCanExecutes();
                },
                canExecute: () => {
                    return (Entry == "0");
                });
        }

        private void RefreshCanExecutes() {
            ((Command)AnswerCommand).ChangeCanExecute();
            ((Command)BackspaceCommand).ChangeCanExecute();
            ((Command)DigitCommand).ChangeCanExecute();
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
    }
}
