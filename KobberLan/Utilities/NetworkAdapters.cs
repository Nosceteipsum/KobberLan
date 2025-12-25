using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using KobberLan.Models;

namespace KobberLan.Utilities;

public static class NetworkAdapters
{
    public static List<NetworkAdapterInfo> GetUsableIPv4Adapters()
    {
        var list = new List<NetworkAdapterInfo>();

        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up) continue;
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel) continue;

            IPInterfaceProperties props;
            try { props = ni.GetIPProperties(); }
            catch { continue; }

            var ipv4 = props.UnicastAddresses
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.Address)
                .FirstOrDefault(a => !IsApipa(a));

            if (ipv4 is null) continue;

            list.Add(new NetworkAdapterInfo
            {
                Id = ni.Id,
                Name = ni.Name,
                Description = ni.Description,
                IPv4 = ipv4
            });
        }

        return list.OrderBy(a => a.Name).ToList();
    }

    private static bool IsApipa(IPAddress ip)
        => ip.GetAddressBytes() is { Length: 4 } b && b[0] == 169 && b[1] == 254;
}