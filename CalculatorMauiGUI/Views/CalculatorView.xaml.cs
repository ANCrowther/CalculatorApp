using CalculatorMauiGUI.ViewModels;

namespace CalculatorMauiGUI.Views;

public partial class CalculatorView : ContentPage
{
	public CalculatorView()
	{
		InitializeComponent();

		this.BindingContext = new CalculatorViewModel();
	}
}