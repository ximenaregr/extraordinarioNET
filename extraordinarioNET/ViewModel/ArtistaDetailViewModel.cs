using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using extraordinarioNET.Servicios;
using extraordinarioNET.Model;

namespace extraordinarioNET.ViewModel
{
    [QueryProperty(nameof(IdArtista), "Idartista")]
    public class ArtistaDetailViewModel : BaseViewModel
    {
        public readonly BaseDeDatos _databaseService;
        public Artista _artista;
        public string _Idartista;

        public ArtistaDetailViewModel(BaseDeDatos BaseDeDatos)
        {
            _databaseService = BaseDeDatos;
            Canciones = new ObservableCollection<Cancion>();
            LoadArtistCommand = new Command(async () => await LoadArtist(GetIdArtista()));
            SongSelectedCommand = new Command<Cancion>(async (cancion) => await OnSongSelected(cancion));
            BackCommand = new Command(async () => await OnBack());
        }

        public string IdArtista
        {
            get => _Idartista;
            set
            {
                _Idartista = value;
                LoadArtistCommand.Execute(null);
            }
        }

        public Artista Artista
        {
            get => _artista;
            set => SetProperty(ref _artista, value);
        }

        public ObservableCollection<Cancion> Canciones { get; }
        public ICommand LoadArtistCommand { get; }
        public ICommand SongSelectedCommand { get; }
        public ICommand BackCommand { get; }

        private string GetIdArtista()
        {
            return IdArtista;
        }

        public async Task LoadArtist(string idArtista)
        {
            if (string.IsNullOrEmpty(IdArtista)) return;

            IsBusy = true;
            try
            {
                var artistId = int.Parse(IdArtista);
                Artista = await _databaseService.GetArtistaAsync(artistId);
                Title = Artista?.Nombre ?? "Artista";

                Canciones.Clear();
                var canciones = await _databaseService.GetCancionesPorArtistaAsync(artistId);
                foreach (var cancion in canciones)
                {
                    Canciones.Add(cancion);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnSongSelected(Cancion cancion)
        {
            if (cancion == null) return;
            await Shell.Current.GoToAsync($"songdetail?songId={cancion.Id}");
        }

        public async Task OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}