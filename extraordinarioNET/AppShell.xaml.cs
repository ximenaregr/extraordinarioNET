using extraordinarioNET.Views;
namespace extraordinarioNET;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("artisdetail", typeof(ArtistaPage));
        Routing.RegisterRoute("songdetail", typeof(CancionPage));
    }
}
