using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;
using KobberLan.Utilities;

namespace KobberLan.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<NetworkAdapterInfo> Adapters { get; } = new();
        public ObservableCollection<GameCard> Games { get; } = new();
        
        [ObservableProperty] private string windowTitle = "KobberLan";
        [ObservableProperty] private NetworkAdapterInfo? selectedAdapter;        
        
        public MainWindowViewModel()
        {
            var asm = typeof(App).Assembly.GetName().Name;
            var uri = new Uri($"avares://{asm}/Assets/covermissing.jpg");
            using var s = AssetLoader.Open(uri);
            var bmp = new Bitmap(s);
            
            Games.Add(new GameCard
            {
                Title = "Warcraft III",
                Cover = bmp,
                Likes = 0,
                Players = 1
            });
            
            Games.Add(new GameCard
            {
                Title = "Doom 2",
                Cover = bmp,
                Likes = 4,
                Players = 2
            });
            
            RefreshAdapters();
            RefreshTitle();
        }
        
        [RelayCommand]
        private void RefreshAdapters()
        {
            Adapters.Clear();
            foreach (var adapterInfo in NetworkAdapters.GetUsableIPv4Adapters())
            {
                Adapters.Add(adapterInfo);
            }

            if (SelectedAdapter is not null)
            {
                var stillThere = Adapters.FirstOrDefault(x => x.Id == SelectedAdapter.Id);
                if (stillThere is not null)
                {
                    SelectedAdapter = stillThere;
                }
            }

            SelectedAdapter ??= Adapters.FirstOrDefault();
        }        
        
        [RelayCommand]
        private void Like(GameCard game) => game.Likes++;

        [RelayCommand]
        private void Share(GameCard game) { /* ... */ }

        [RelayCommand]
        private void Clear(GameCard game) { /* ... */ }

        [RelayCommand]
        private void Play(GameCard game) { /* ... */ }


        public void RefreshTitle()
        {
            WindowTitle = $"KobberLan - IP:{SelectedAdapter?.IPv4}";
        }
        
        public void StartDiscovery()
        {
            AppLog.Info("Starting discovery...");
        }
        
        public void StopDiscovery()
        {
            AppLog.Info("Stop discovery...");
        }
    }
}
