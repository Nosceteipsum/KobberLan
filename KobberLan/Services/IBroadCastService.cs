using System;
using System.Net;
using System.Threading.Tasks;
using KobberLan.Models;

namespace KobberLan.Services;

public interface IBroadCastService
{
    public event Action<IPAddress>? OnPeerUp;
    public event Action<IPAddress>? OnPeerDown;
    
    public NetworkAdapterInfo? SelectedAdapter { get; set; }
    
    public Task BroadcastSearchAsync();
    public void RefreshAdapters();
    public Task StopDiscovery();
    public Task StartDiscovery();
}