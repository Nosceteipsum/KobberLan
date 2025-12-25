using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using KobberLan.Utilities;
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
            try
            {
                var dlg = new AboutWindow();
                await dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                AppLog.Error("About_Click exception.",ex);
            }
        }
        
        private async void Interface_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new InterfaceWindow();
                await dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                AppLog.Error("Interface_Click exception.",ex);
            }
        }
        
        private void ShowLog_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new LogWindow().ShowDialog(this);
        }        
    }
}