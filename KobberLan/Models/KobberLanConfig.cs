using System.Text.Json.Serialization;

namespace KobberLan.Models;

public class KobberLanConfig
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("image")] public string? Image { get; set; }

    [JsonPropertyName("startGame")] public string? StartGame { get; set; }
    [JsonPropertyName("startGameParams")] public string? StartGameParams { get; set; }

    [JsonPropertyName("startServer")] public string? StartServer { get; set; }
    [JsonPropertyName("startServerParams")] public string? StartServerParams { get; set; }

    [JsonPropertyName("version")] public string? Version { get; set; }
    [JsonPropertyName("maxPlayers")] public int MaxPlayers { get; set; }
}