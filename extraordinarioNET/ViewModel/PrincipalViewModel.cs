using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace extraordinarioNET.ViewModel
{
    public class PrincipalViewModel : BaseViewModel
    {
        public PrincipalViewModel()
        {
            Title = "Top 10 Artistas";
            StartCommand = new Command(async () => await OnStartClicked());
        }
        public ICommand StartCommand { get; }
        private async Task OnStartClicked()
        {
            await Shell.Current.GoToAsync("//artistas");
        }
    }
}
