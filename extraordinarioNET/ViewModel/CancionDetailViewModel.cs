using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using extraordinarioNET.Model;
using extraordinarioNET.Servicios;

namespace extraordinarioNET.ViewModel
{
    [QueryProperty(nameof(IdCancion), "Idcancion")]
    public class CancionDetailViewModel : BaseViewModel
    {
        private readonly BaseDeDatos _databaseService;
        private Cancion _cancion;
        private string _Idcancion;

        public CancionDetailViewModel(BaseDeDatos databaseService)
        {
            _databaseService = databaseService;
            LoadSongCommand = new Command(async () => await LoadSong());
            BackCommand = new Command(async () => await OnBack());
        }

        public string IdCancion
        {
            get => _Idcancion;
            set
            {
                _Idcancion = value;
                LoadSongCommand.Execute(null);
            }
        }

        public Cancion Cancion
        {
            get => _cancion;
            set => SetProperty(ref _cancion, value);
        }

        public ICommand LoadSongCommand { get; }
        public ICommand BackCommand { get; }

        private async Task LoadSong()
        {
            if (string.IsNullOrEmpty(IdCancion)) return;

            IsBusy = true;
            try
            {
                var songId = int.Parse(IdCancion);
                Cancion = await _databaseService.GetSongAsync(songId);
                Title = Cancion?.Nombre ?? "Canción";
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

        private async Task OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}