using extraordinarioNET.Servicios;
using extraordinarioNET.ViewModel;

namespace extraordinarioNET.Views
{

	public partial class ArtistaPage : ContentPage
	{
		public ArtistaPage()
		{
			InitializeComponent();
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "musica.db3");
            var databaseService = new BaseDeDatos(dbPath);
            BindingContext = new ArtistaDetailViewModel(databaseService);
		}
	}
}