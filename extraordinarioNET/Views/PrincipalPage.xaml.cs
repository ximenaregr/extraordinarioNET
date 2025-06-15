using extraordinarioNET.ViewModel;
namespace extraordinarioNET.Views;

public partial class PrincipalPage : ContentPage
{
	public PrincipalPage(PrincipalViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }
}