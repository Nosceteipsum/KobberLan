using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;
using KobberLan.Services;
using KobberLan.Utilities;

namespace KobberLan.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<GameCard> Games { get; } = new();
        public ObservableCollection<NetworkAdapterInfo> Adapters { get; } = new();
        
        [ObservableProperty] private string windowTitle = "KobberLan";
        [ObservableProperty] private WindowIcon? windowIcon;
        [ObservableProperty] private LocalGame? selectedSuggestedGame;        
        [ObservableProperty] private NetworkAdapterInfo? selectedAdapter;
        
        private readonly IGameConfigService gameConfigService;
        private readonly ICoverService coverService;
        private readonly IGameLauncher gameLauncher;
        private readonly IBroadCastService broadCastService;
        
        public MainWindowViewModel()
        {
            gameConfigService = new GameConfigService();
            coverService = new CoverService();
            gameLauncher = new GameLauncher();
            broadCastService = new BroadCastService(Adapters);
            
            var asm = typeof(App).Assembly.GetName().Name;
            var uri = new Uri($"avares://{asm}/Assets/mesh0.ico");
            WindowIcon = new WindowIcon(AssetLoader.Open(uri));
            
            RefreshAdapters();
            RefreshTitle();
            InitBroadcast();
        }
        
        partial void OnSelectedAdapterChanged(NetworkAdapterInfo? value)
        {
            broadCastService.SelectedAdapter = value;
        }        
        
        public void SuggestGame(LocalGame game)
        {
            SelectedSuggestedGame = game;
            Games.Add(new GameCard
            {
                Title = game.Title,
                Key = game.Key,
                FolderPath =  game.FolderPath,
                Cover = coverService.LoadCover(game),
                Likes = 0,
                Players = 0
            });
            
            // todo: UDP broadcast new game
            
        }        
        
        [RelayCommand]
        private void RefreshAdapters() => broadCastService.RefreshAdapters();
        
        [RelayCommand]
        private void Like(GameCard game) => game.Likes++;

        [RelayCommand]
        private void Share(GameCard game) { /* ... */ }

        [RelayCommand]
        private void Clear(GameCard game) { /* ... */ }
        
        [RelayCommand(CanExecute = nameof(CanPlay))]
        private void Play(GameCard? game)
        {
            if (game is null)
            {
                AppLog.Error("game is null");
                return;
            }

            AppLog.Info("Start game: " + game.Title + " Folder: " + game.FolderPath);
            gameConfigService.TryLoad(game.FolderPath);
            gameLauncher.Play(game.FolderPath, gameConfigService.GetGameFullPath());
        }

        private bool CanPlay(GameCard? game) => game is not null;        
        
        public void RefreshTitle()
        {
            WindowTitle = $"KobberLan - IP:{SelectedAdapter?.IPv4} - Players: {broadCastService.GetPlayerIps().Count}";
            
            //Refresh icon
            var asm = typeof(App).Assembly.GetName().Name;
            var iconName = broadCastService.GetPlayerIps().Count <= 9 ? $"mesh{broadCastService.GetPlayerIps().Count}.ico" : "meshX.ico";
            var uri = new Uri($"avares://{asm}/Assets/{iconName}");
            WindowIcon = new WindowIcon(AssetLoader.Open(uri));
        }
        
        public IBroadCastService GetBroadCastService() => broadCastService;
        
        private void InitBroadcast()
        {
            broadCastService.OnPeerUp += ip => Dispatcher.UIThread.Post(() => broadCastService.AddPlayerIp(ip));
            broadCastService.OnPeerDown += ip => Dispatcher.UIThread.Post(() => broadCastService.RemovePlayerIp(ip));     
        }
        
    }
}
