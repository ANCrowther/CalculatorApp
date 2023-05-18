namespace CalculatorMauiGUI.Resources;
public interface ICommand {
    public void Execute(Object parameter);
    public bool CanExecute(Object parameter);
    public event EventHandler CanExecuteChanged;
}
