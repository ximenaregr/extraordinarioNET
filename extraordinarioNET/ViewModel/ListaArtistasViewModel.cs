using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using extraordinarioNET.Model;
using extraordinarioNET.Servicios;

namespace extraordinarioNET.ViewModel
{
    public class ListaArtistasViewModel : BaseViewModel
    {
        private readonly BaseDeDatos _databaseService;

        public ListaArtistasViewModel(BaseDeDatos databaseService)
        {
            _databaseService = databaseService;
            Title = "Top 10 Artistas";
            Artistas = new ObservableCollection<Artista>();
            LoadArtistasCommand = new Command(async () => await LoadArtista());
            ArtistaSelectedCommand = new Command<Artista>(async (artist) => await OnArtistaSelected(artist));
        }
        public ObservableCollection<Artista> Artistas { get; }
        public ICommand LoadArtistasCommand { get; }
        public ICommand ArtistaSelectedCommand { get; }

      

        private async Task OnArtistaSelected(Artista artista)
        {
            if (artista == null) return;
            await Shell.Current.GoToAsync($"artista-detail?Idartista={artista.Id}");
        }


        public async Task LoadArtista()
        {
            IsBusy = true;
            try
            {
                Artistas.Clear();
                var artistas = await _databaseService.GetArtistasAsync();

                // Debug: Imprime información
                System.Diagnostics.Debug.WriteLine($"Cargando artistas...");
                System.Diagnostics.Debug.WriteLine($"Artistas encontrados: {artistas?.Count() ?? 0}");

                foreach (var artista in artistas)
                {
                    System.Diagnostics.Debug.WriteLine($"Artista: {artista.Nombre}, Ranking: {artista.Ranking}");
                    Artistas.Add(artista);
                }

                System.Diagnostics.Debug.WriteLine($"Artistas en colección: {Artistas.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
