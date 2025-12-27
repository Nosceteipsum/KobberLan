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

        private async void ResendBroadcast_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    await vm.GetDiscoveryService().BroadcastSearchAsync();
                }
            }
            catch (Exception ex)
            {
                AppLog.Error("ResendBroadcast_Click exception.",ex);
            }
        }
        
        private async void SuggestGame_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var w = new SuggestGameWindow
                {
                    DataContext = new SuggestGameWindowViewModel()
                };

                var ok = await w.ShowDialog<bool>(this);
                if (ok && w.Result is not null)
                {
                    // Todo: Her har du valgt et spil (w.Result)
                    // Todo: Næste step: send DTO_Suggestion broadcast
                    // Todo: fx: (DataContext as MainWindowViewModel)?.SuggestGame(w.Result);
                }
            }
            catch (Exception ex)
            {
                AppLog.Error("SuggestGame_Click exception.",ex);
            }
        }        
        
        private async void Interface_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var w = new InterfaceWindow();
                w.DataContext = DataContext;
                await w.ShowDialog(this);
                (DataContext as MainWindowViewModel)?.RefreshTitle();
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