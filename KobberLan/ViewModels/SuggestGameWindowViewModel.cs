using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;

namespace KobberLan.ViewModels;

public record GameFolder(string Name, string Path, bool IsAll = false);

public partial class SuggestGameWindowViewModel : ViewModelBase
{
    public ObservableCollection<GameFolder> Folders { get; } = new();
    public ObservableCollection<LocalGame> FilteredGames { get; } = new();

    [ObservableProperty] private GameFolder? selectedFolder;
    [ObservableProperty] private LocalGame? selectedGame;

    public SuggestGameWindowViewModel()
    {
        Refresh();
    }

    [RelayCommand]
    private void Refresh()
    {
        Folders.Clear();
        FilteredGames.Clear();
        SelectedGame = null;

        var gamesRoot = Path.Combine(AppContext.BaseDirectory, "Games");
        if (!Directory.Exists(gamesRoot))
            return;

        // "All games" folder (virtuel)
        Folders.Add(new GameFolder("All games", gamesRoot, IsAll: true));

        // Kategori-foldere direkte under Games\
        foreach (var catDir in Directory.GetDirectories(gamesRoot).OrderBy(Path.GetFileName))
        {
            var catName = Path.GetFileName(catDir);
            Folders.Add(new GameFolder(catName, catDir));
        }

        SelectedFolder = Folders.FirstOrDefault();
    }

    partial void OnSelectedFolderChanged(GameFolder? value)
    {
        FilteredGames.Clear();
        SelectedGame = null;

        if (value is null) return;

        var games = value.IsAll
            ? LoadAllGames(value.Path)
            : LoadGamesInFolder(value.Path);

        foreach (var g in games)
            FilteredGames.Add(g);

        SelectedGame = FilteredGames.FirstOrDefault();
    }

    private static IEnumerable<LocalGame> LoadAllGames(string gamesRoot)
    {
        // All games = alle spil-foldere i alle kategori-foldere + eventuelle direkte under Games\
        // 1) Spil direkte under Games\
        foreach (var g in LoadGamesInFolder(gamesRoot))
            yield return g;

        // 2) Spil under kategori-foldere
        foreach (var catDir in Directory.GetDirectories(gamesRoot))
        {
            foreach (var g in LoadGamesInFolder(catDir))
                yield return g;
        }
    }

    private static IEnumerable<LocalGame> LoadGamesInFolder(string folder)
    {
        // Antag: hver spil ligger som subfolder i den valgte folder
        // fx Games\Strategy\Warcraft3\...
        if (!Directory.Exists(folder))
            yield break;

        foreach (var gameDir in Directory.GetDirectories(folder).OrderBy(Path.GetFileName))
        {
            var key = Path.GetFileName(gameDir);
            var title = key; // v1: senere kan du læse titel fra config.json i folderen

            var cover = TryLoadCover(gameDir);

            yield return new LocalGame
            {
                Key = key,
                Title = title,
                FolderPath = gameDir,
                Cover = cover
            };
        }
    }

    private static Bitmap? TryLoadCover(string gameDir)
    {
        // Forsøg cover.* i folderen
        var candidates = new[]
        {
            Path.Combine(gameDir, "cover.jpg"),
            Path.Combine(gameDir, "cover.jpeg"),
            Path.Combine(gameDir, "cover.png"),
            Path.Combine(gameDir, "cover.webp")
        };

        var coverPath = candidates.FirstOrDefault(File.Exists);
        if (coverPath is null) return null;

        try
        {
            using var s = File.OpenRead(coverPath);
            return new Bitmap(s);
        }
        catch
        {
            return null;
        }
    }

    [RelayCommand]
    private void SelectGame(LocalGame game)
    {
        SelectedGame = game;
    }
}
