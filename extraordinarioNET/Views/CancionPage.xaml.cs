using extraordinarioNET.Servicios;
using extraordinarioNET.ViewModel;
namespace extraordinarioNET.Views;

public partial class CancionPage : ContentPage
{
	public CancionPage()
	{
		InitializeComponent();
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "musica.db3");
        var databaseService = new BaseDeDatos(dbPath);
        BindingContext = new CancionDetailViewModel(databaseService);
    }
}