using System.Net;

namespace KobberLan.Models;

public class NetworkAdapterInfo
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public IPAddress? IPv4 { get; init; }

    public override string ToString()
        => IPv4 is null ? Name : $"{Name} ({IPv4})";
}