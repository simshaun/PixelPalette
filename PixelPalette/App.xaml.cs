using System.Linq;
using System.Windows;
using PixelPalette.State;

namespace PixelPalette
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            EphemeralState.Data.DebugMode = e.Args.Contains("-debug");
        }
    }
}