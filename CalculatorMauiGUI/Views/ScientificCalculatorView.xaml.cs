using CalculatorMauiGUI.ViewModels;

namespace CalculatorMauiGUI.Views;

public partial class ScientificCalculatorView : ContentPage
{
	public ScientificCalculatorView() {
        InitializeComponent();

        this.BindingContext = new CalculatorViewModel();
    }
}