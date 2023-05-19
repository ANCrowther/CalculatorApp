using ShuntingYardLibrary;
using CalculatorSupportLibrary;
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
                    Entry += MathValues.PI.ToString();
                } else if (arg == "e") {
                    Entry += MathValues.E.ToString();
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
                if (MathLogic.IsOperator(Entry) && Entry[0] != '-' && arg != "(") {
                    Entry = "0";
                }
                RefreshCanExecutes();
            },
            canExecute: (string arg) => {
                return !(arg == "." && entry.IsDecimalInRecentNumber());
            });

        NegativeDigitCommand = new Command(
            execute: () => {
                Entry = MathLogic.ChangeSign(Entry);
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
}
