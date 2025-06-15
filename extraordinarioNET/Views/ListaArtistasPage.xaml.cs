using extraordinarioNET.ViewModel;
using extraordinarioNET.Servicios;
namespace extraordinarioNET.Views;


public partial class ListaArtistasPage : ContentPage
{
	public ListaArtistasPage()
	{
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "musica.db3");
        var dbService = new BaseDeDatos(dbPath);
        BindingContext = new ListaArtistasViewModel(dbService);
    }
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if(BindingContext is ListaArtistasViewModel viewModel)
		{
			await viewModel.LoadArtista();
		}
	}
}