using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;

namespace KobberLan.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<GameCard> Games { get; } = new();
        [ObservableProperty] private string windowTitle = "KobberLan";
        
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
        }
        
        [RelayCommand]
        private void Like(GameCard game) => game.Likes++;

        [RelayCommand]
        private void Share(GameCard game) { /* ... */ }

        [RelayCommand]
        private void Clear(GameCard game) { /* ... */ }

        [RelayCommand]
        private void Play(GameCard game) { /* ... */ }


        public void StartDiscovery()
        {
            Console.WriteLine("Starter discovery...");
            WindowTitle = "KobberLan - Scanning...";
        }
        
        public void StopDiscovery()
        {
            Console.WriteLine("Stop discovery...");
            
        }
    }
}
