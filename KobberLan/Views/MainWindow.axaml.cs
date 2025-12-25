using Avalonia.Controls;
using Avalonia.Interactivity;
using KobberLan.ViewModels;

namespace KobberLan.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Opened += (_, _) => (DataContext as MainWindowViewModel)?.StartDiscovery();
            Closed += (_, _) => (DataContext as MainWindowViewModel)?.StopDiscovery();            
        }
        
        private async void About_Click(object? sender, RoutedEventArgs e)
        {
            var dlg = new AboutWindow();
            await dlg.ShowDialog(this);
        }        
    }
}