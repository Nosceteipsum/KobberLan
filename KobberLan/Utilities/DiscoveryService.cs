using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KobberLan.Utilities;

public sealed class DiscoveryService : IAsyncDisposable
{
    private const string MSG_SEARCH = "B";
    private const string MSG_DISCONNECT = "D";
    private const string MSG_PORTCHECK = "P";

    private readonly int _port;
    private readonly HashSet<IPAddress> _peers = new();
    private readonly object _lock = new();

    private CancellationTokenSource? _cts;
    private Task? _listenTask;

    private UdpClient? _listener;
    private UdpClient? _sender;

    private IPAddress? _localIp;
    private IPAddress? _broadcastIp;

    public bool FirewallSeemsOk { get; private set; } = false;

    public event Action<IPAddress>? OnPeerUp;
    public event Action<IPAddress>? OnPeerDown;

    public DiscoveryService(int port)
    {
        _port = port;
    }

    public void Start(IPAddress localIp)
    {
        if (_cts is not null)
        {
            AppLog.Warn("Broadcast, Already started.");
            return;
        }

        _localIp = localIp;
        _broadcastIp = GetBroadcastAddress(localIp);

        _cts = new CancellationTokenSource();
        _listener = new UdpClient(new IPEndPoint(IPAddress.Any, _port));

        _sender = new UdpClient();
        _sender.EnableBroadcast = true;

        AppLog.Info($"Discovery started. Local={_localIp} Broadcast={_broadcastIp} Port={_port}");
        _listenTask = ListenLoopAsync(_cts.Token);
    }

    public async Task StopAsync()
    {
        if (_cts is null)
        {
            AppLog.Warn("Broadcast, Already stopped.");
            return;
        }

        try
        {
            await SendAsync(MSG_DISCONNECT);
        }
        catch { /* ignore shutdown errors */ }

        await _cts.CancelAsync();

        try
        {
            if (_listenTask is not null)
            {
                await _listenTask;
            }
        }
        catch { /* ignore */ }

        _listener?.Close();
        _sender?.Close();
        _listener = null;
        _sender = null;

        _cts.Dispose();
        _cts = null;

        AppLog.Info("Discovery stopped.");
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }

    public Task BroadcastSearchAsync() => SendAsync(MSG_SEARCH);

    private async Task SendAsync(string msg)
    {
        if (_sender is null || _broadcastIp is null) return;

        var data = Encoding.ASCII.GetBytes(msg);
        await _sender.SendAsync(data, data.Length, new IPEndPoint(_broadcastIp, _port));
        AppLog.Info($"Broadcast sent: '{msg}' -> {_broadcastIp}:{_port}");
    }

    private async Task ListenLoopAsync(CancellationToken ct)
    {
        if (_listener is null) return;
        while (!ct.IsCancellationRequested)
        {
            UdpReceiveResult res;

            try
            {
                res = await _listener.ReceiveAsync().WaitAsync(ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (SocketException ex)
            {
                AppLog.Error("SocketException in listen loop.", ex);
                break;
            }
            catch (Exception ex)
            {
                AppLog.Error("Exception in listen loop.", ex);
                break;
            }

            var fromIp = res.RemoteEndPoint.Address;
            var msg = Encoding.ASCII.GetString(res.Buffer);
            HandleMessage(msg, fromIp);
        }
    }

    private void HandleMessage(string msg, IPAddress fromIp)
    {
        if (_localIp is null)
        {
            AppLog.Warn("Local IP address is null.");
            return;
        }

        lock (_lock)
        {
            if (msg == MSG_PORTCHECK)
            {
                AppLog.Info("Portcheck received (ignored).");
                return;
            }

            if (msg == MSG_DISCONNECT)
            {
                if (fromIp.Equals(_localIp))
                {
                    AppLog.Info("Disconnect from self ignored.");
                    return;
                }

                if (_peers.Remove(fromIp))
                {
                    AppLog.Info($"Peer disconnected: {fromIp}");
                    OnPeerDown?.Invoke(fromIp);
                }
                return;
            }

            if (msg == MSG_SEARCH)
            {
                if (fromIp.Equals(_localIp))
                {
                    FirewallSeemsOk = true;
                    AppLog.Info("Received own SEARCH back (firewall likely OK).");
                    return;
                }

                if (_peers.Add(fromIp))
                {
                    AppLog.Info($"New peer found: {fromIp}");
                    OnPeerUp?.Invoke(fromIp);
                }
                return;
            }

            AppLog.Warn($"Unknown message '{msg}' from {fromIp}");
        }
    }

    private static IPAddress GetBroadcastAddress(IPAddress localIp)
    {
        var ni = NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.OperationalStatus == OperationalStatus.Up)
            .SelectMany(n => n.GetIPProperties().UnicastAddresses
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => new { a.Address, a.IPv4Mask }))
            .FirstOrDefault(x => x.Address.Equals(localIp));

        var mask = ni?.IPv4Mask;
        if (mask is null)
        {
            // fallback: "global" broadcast (often works, but not always on all setups)
            return IPAddress.Broadcast;
        }

        var ipBytes = localIp.GetAddressBytes();
        var maskBytes = mask.GetAddressBytes();
        var bcBytes = new byte[4];

        for (int i = 0; i < 4; i++)
        {
            bcBytes[i] = (byte)(ipBytes[i] | (maskBytes[i] ^ 255));
        }

        return new IPAddress(bcBytes);
    }

}