using extraordinarioNET.Servicios;
namespace extraordinarioNET
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "artists.db3");
            BaseDeDatos BaseDeDatos = new BaseDeDatos(dbPath);
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

      
            window.Title = "Top 10 Artistas";
            return new Window(new AppShell());
        }
    }
}