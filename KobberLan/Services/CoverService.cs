using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using KobberLan.Models;
using KobberLan.Utilities;

namespace KobberLan.Services;

public class CoverService : ICoverService
{
    public Bitmap LoadCover(LocalGame game)
    {
        try
        {
            var coverPath = Path.Combine(game.FolderPath, "_kobberlan.jpg");

            if (File.Exists(coverPath))
            {
                using var fs = File.OpenRead(coverPath);
                return new Bitmap(fs);
            }
        }
        catch (Exception ex)
        {
            AppLog.Error($"Game {game.Title} cover load error", ex);
        }

        using var s = AssetLoader.Open(new Uri("avares://KobberLan/Assets/covermissing.jpg"));
        return new Bitmap(s);
    }
}