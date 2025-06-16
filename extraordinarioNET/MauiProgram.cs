using Microsoft.Extensions.Logging;
using extraordinarioNET.Servicios;
using extraordinarioNET.ViewModel;
using extraordinarioNET.Views;

namespace extraordinarioNET
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Inicializar SQLite (importante para Android)
            SQLitePCL.Batteries_V2.Init();

            // Database
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "musica.db3");

            // Registrar BaseDeDatos como singleton
            builder.Services.AddSingleton<BaseDeDatos>(serviceProvider =>
            {
                var database = new BaseDeDatos(dbPath);
                _ = Task.Run(async () => await database.InitializeAsync());
                return database;
            });

            // ViewModels
            builder.Services.AddTransient<PrincipalViewModel>();
            builder.Services.AddTransient<ListaArtistasViewModel>();
            builder.Services.AddTransient<CancionDetailViewModel>();

            // Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ListaArtistasPage>();
            builder.Services.AddTransient<ArtistaPage>();
            builder.Services.AddTransient<CancionPage>();

            // RUTAS
            Routing.RegisterRoute("artista-detail", typeof(ArtistaPage));
            Routing.RegisterRoute("cancion-detail", typeof(CancionPage));

            builder.Logging.AddDebug();
            return builder.Build();
        }
    }
}