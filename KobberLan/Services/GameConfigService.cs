using System;
using System.IO;
using System.Text.Json;
using KobberLan.Models;
using KobberLan.Utilities;

namespace KobberLan.Services;

public class GameConfigService : IGameConfigService
{
    private KobberLanConfig? kobberLanConfig;
    private string gameFolder = string.Empty;

    public KobberLanConfig? TryLoad(string folder)
    {
        gameFolder = folder;
        try
        {
            var path = Path.Combine(gameFolder, "_kobberlan.config");
            if (!File.Exists(path)) return null;

            var json = File.ReadAllText(path);
            kobberLanConfig = JsonSerializer.Deserialize<KobberLanConfig>(json, Options);
            return kobberLanConfig;
        }
        catch (Exception ex)
        {
            AppLog.Error("Error reading game data from config, _kobberlan.config", ex);
        }

        return null;
    }

    public string GetGameFullPath()
    {
        if (string.IsNullOrWhiteSpace(gameFolder) || string.IsNullOrEmpty(kobberLanConfig?.StartGame))
        {
            AppLog.Error($"gameFolder ({gameFolder}) or StartGamePath({kobberLanConfig?.StartGame}) is empty");
            return  string.Empty;
        }
        
        return Path.Combine(gameFolder, kobberLanConfig.StartGame);
    }
    
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
    };
}