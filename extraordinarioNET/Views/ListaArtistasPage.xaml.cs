using extraordinarioNET.ViewModel;
using extraordinarioNET.Servicios;

namespace extraordinarioNET.Views;

public partial class ListaArtistasPage : ContentPage
{
    private readonly ListaArtistasViewModel _viewModel;

    public ListaArtistasPage(ListaArtistasViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadArtista();
    }
}