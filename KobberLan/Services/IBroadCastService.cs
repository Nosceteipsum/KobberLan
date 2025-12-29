using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KobberLan.Models;

namespace KobberLan.Services;

public interface IBroadCastService
{
    public event Action<IPAddress>? OnPeerUp;
    public event Action<IPAddress>? OnPeerDown;
    
    public NetworkAdapterInfo? SelectedAdapter { get; set; }

    public List<string> GetPlayerIps();
    public void AddPlayerIp(IPAddress ip);
    public void RemovePlayerIp(IPAddress ip);
    
    public Task BroadcastSearchAsync();
    public void RefreshAdapters();
    public Task StopDiscovery();
    public Task StartDiscovery();
}