using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;

namespace KobberLan.ViewModels;

public partial class SuggestGameWindowViewModel : ViewModelBase
{
    public ObservableCollection<LocalGame> Games { get; } = new();

    [ObservableProperty] private LocalGame? selectedGame;

    [RelayCommand]
    private void Refresh()
    {
        Games.Clear();

        // Games folder ved siden af exe (eller working dir)
        var baseDir = AppContext.BaseDirectory;
        var gamesDir = Path.Combine(baseDir, "Games");

        if (!Directory.Exists(gamesDir))
            return;

        foreach (var dir in Directory.GetDirectories(gamesDir))
        {
            var key = Path.GetFileName(dir);
            var title = key; // v1: map senere til pæn titel fra config

            // valgfrit cover: cover.jpg / cover.png i game folder
            Bitmap? bmp = null;
            var coverJpg = Path.Combine(dir, "cover.jpg");
            var coverPng = Path.Combine(dir, "cover.png");
            var coverPath = File.Exists(coverJpg) ? coverJpg : (File.Exists(coverPng) ? coverPng : null);

            if (coverPath is not null)
            {
                using var s = File.OpenRead(coverPath);
                bmp = new Bitmap(s);
            }

            Games.Add(new LocalGame
            {
                Key = key,
                Title = title,
                FolderPath = dir,
                Cover = bmp
            });
        }

        SelectedGame ??= Games.Count > 0 ? Games[0] : null;
    }
}