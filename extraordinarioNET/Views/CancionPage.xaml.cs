using extraordinarioNET.ViewModel;

namespace extraordinarioNET.Views;

public partial class CancionPage : ContentPage
{
    public CancionPage(CancionDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}