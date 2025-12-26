using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;
using KobberLan.Utilities;

namespace KobberLan.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> Players { get; } = new();
        public ObservableCollection<NetworkAdapterInfo> Adapters { get; } = new();
        public ObservableCollection<GameCard> Games { get; } = new();
        
        [ObservableProperty] private string windowTitle = "KobberLan";
        [ObservableProperty] private NetworkAdapterInfo? selectedAdapter;
        [ObservableProperty] private WindowIcon? windowIcon;
        private readonly DiscoveryService discovery = new(port: 50000);
        
        public MainWindowViewModel()
        {
            var asm = typeof(App).Assembly.GetName().Name;
            var uri = new Uri($"avares://{asm}/Assets/mesh0.ico");
            WindowIcon = new WindowIcon(AssetLoader.Open(uri));
            
            //Todo: Temporary Dummy data, remove when implemented 
            uri = new Uri($"avares://{asm}/Assets/covermissing.jpg");
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
            InitBroadcast();
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
            WindowTitle = $"KobberLan - IP:{SelectedAdapter?.IPv4} - Players: {Players.Count}";
            
            //Refresh icon
            var asm = typeof(App).Assembly.GetName().Name;
            var iconName = Players.Count <= 9 ? $"mesh{Players.Count}.ico" : "meshX.ico";
            var uri = new Uri($"avares://{asm}/Assets/{iconName}");
            WindowIcon = new WindowIcon(AssetLoader.Open(uri));
        }
        
        public DiscoveryService GetDiscoveryService() => discovery;
        
        
        public async Task StartDiscovery()
        {
            AppLog.Info("Starting discovery...");
            if (SelectedAdapter?.IPv4 == null)
            {
                AppLog.Warn("SelectedAdapter is null, can't send broadcast");
                return;
            }

            try
            {
                discovery.Start(SelectedAdapter.IPv4);
                await discovery.BroadcastSearchAsync();
            }
            catch (Exception ex)
            {
                AppLog.Error("Error starting discovery", ex);
            }
        }
        
        public async Task StopDiscovery()
        {
            AppLog.Info("Stop discovery...");
            await discovery.StopAsync();
        }

        private void InitBroadcast()
        {
            discovery.OnPeerUp += ip => Dispatcher.UIThread.Post(() => AddIp(ip));
            discovery.OnPeerDown += ip => Dispatcher.UIThread.Post(() => RemoveIp(ip));     
        }

        private void AddIp(IPAddress ip)
        {
            if (Players.Contains(ip.ToString()))
            {
                AppLog.Warn("OnPeerUp, IP already exist: " + ip);
            }
            else
            {
                Players.Add(ip.ToString());
            }
        }

        private void RemoveIp(IPAddress ip)
        {
            if (Players.Contains(ip.ToString()))
            {
                Players.Remove(ip.ToString());
            }
            else
            {
                AppLog.Warn("OnPeerDown, IP does not exist? " + ip);
            }            
        }
        
    }
}
