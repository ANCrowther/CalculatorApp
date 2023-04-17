using ShuntingYardLibrary;

namespace CalculatorGUI.Logic;
public static class CalculatorLogic {
    public static string input { get; private set; } = String.Empty;
    public static string output { get; private set; } = String.Empty;
    private static List<char> list = new List<char>();

    public static void ClickOne() {
       UpdateString('1');
    }

    public static void ClickTwo() {
        UpdateString('2');
    }

    public static void ClickThree() {
        UpdateString('3');
    }

    public static void ClickFour() {
        UpdateString('4');
    }

    public static void ClickFive() {
        UpdateString('5');
    }

    public static void ClickSix() {
        UpdateString('6');
    }

    public static void ClickSeven() {
        UpdateString('7');
    }

    public static void ClickEight() {
        UpdateString('8');
    }

    public static void ClickNine() {
        UpdateString('9');
    }

    public static void ClickZero() {
        UpdateString('0');
    }

    public static void ClickDecimal() {
        if (NoDecimalUsedInNumber()) {
            if (!int.TryParse(input[input.Length - 1].ToString(), out _)) {
                UpdateString('0');
            }
            UpdateString('.');
        }
    }

    public static void ClickAddition() {
        UpdateString('+');
    }

    public static void ClickSubtraction() {
        UpdateString('-');
    }

    public static void ClickMultiplication() {
        UpdateString('*');
    }

    public static void ClickDivision() {
        UpdateString('/');
    }

    public static void ClickExponent() {
        UpdateString('^');
    }

    public static void ClickOpenParenthesis() {
        UpdateString('(');
    }

    public static void ClickClosedParenthesis() {
        UpdateString(')');
    }

    public static void ClickPlusOrMinus() {
        //TODO: check to see if the number is already negative. Currently just adds a new '-' sign.
        for (int i = list.Count - 1; i >= 0; i--) {
            if (i == 0) {
                InsertNegative(i);
                break;
            }
            if (!char.IsNumber(list[i]) && list[i] != '.') {
                InsertNegative(i + 1);
                break;
            }
        }
        UpdateString();
    }

    public static void ClickClear() {
        input = String.Empty;
        output = String.Empty;
        list.Clear();
    }

    public static void ClickBackSpace() {
        if (input != String.Empty) {
            input = input.Remove(input.Length - 1);
            list.RemoveAt(list.Count - 1);
        }
    }

    public static void ClickEquals() {
        output = ExpressionTree.Evaluate(input);
        input = String.Empty;
        list.Clear();
    }

    public static void ClickSaveAnswer() {
        input = output;
        list.Clear();
        foreach (char c in output) {
            list.Add(c);
        }
        if (list[0] == '-') {
            list.RemoveAt(0);
            InsertNegative(0);
        }
        output = String.Empty;
    }

    private static bool NoDecimalUsedInNumber() {
        bool notUsed = true;
        for (int i = list.Count - 1; i >= 0; i--) {
            if (list[i] == '.') {
                notUsed = false;
                break;
            }
            if (!char.IsNumber(list[i])) {
                break;
            }
        }
        return notUsed;
    }

    private static void InsertNegative(int i) {
        list.Insert(i, '-');
        list.Insert(i, '0');
        list.Insert(i, '(');
        i += 3;
        while (list.Count - 1 > i && (char.IsNumber(list[i]) || list[i] == '.')) {
            i++;
        }
        if (list.Count - 1 == i) {
            list.Add(')');
        } else {
            list.Insert(i + 1, ')');
        }
    }

    private static void UpdateString() {
        input = String.Empty;
        foreach (char c in list) {
            input += c;
        }
    }

    private static void UpdateString(char symbol) {
        input = String.Empty;
        list.Add(symbol);
        foreach (char c in list) {
            input += c;
        }
    }
}
