using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KobberLan.Models;
using KobberLan.Utilities;

namespace KobberLan.ViewModels;

public record GameFolder(string Name, string Path, SuggestGameWindowViewModel.FolderKind Kind);

public partial class SuggestGameWindowViewModel : ViewModelBase
{
    public enum FolderKind { All, Uncategorized, Category }
    
    public ObservableCollection<GameFolder> Folders { get; } = new();
    public ObservableCollection<LocalGame> FilteredGames { get; } = new();

    [ObservableProperty] private GameFolder? selectedFolder;
    [ObservableProperty] private LocalGame? selectedGame;
    
    private static readonly Bitmap FallbackCover = LoadFallbackCover();

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

        var root = Path.Combine(AppContext.BaseDirectory, "Games");
        if (!Directory.Exists(root))
        {
            AppLog.Error("Game folder not found");
            return;
        }

        var hasUncategorized = Directory.GetDirectories(root).Any(IsGameFolder);
        Folders.Add(new GameFolder("All games", root, FolderKind.All));
        if (hasUncategorized)
        {
            Folders.Add(new GameFolder("Uncategorized", root, FolderKind.Uncategorized));
        }

        foreach (var dir in Directory.GetDirectories(root).OrderBy(Path.GetFileName))
        {
            if (IsGameFolder(dir)) continue; // spil direkte under Games\
            Folders.Add(new GameFolder(Path.GetFileName(dir), dir, FolderKind.Category));
        }

        SelectedFolder = Folders.FirstOrDefault();
    }


    private static LocalGame MakeGame(string gameDir)
    {
        var key = Path.GetFileName(gameDir);
        return new LocalGame
        {
            Key = key,
            Title = key,
            FolderPath = gameDir,
            Cover = TryLoadCover(gameDir)
        };
    }
    

    partial void OnSelectedFolderChanged(GameFolder? value)
    {
        FilteredGames.Clear();
        SelectedGame = null;
        if (value is null) return;

        IEnumerable<string> gameDirs = value.Kind switch
        {
            FolderKind.All => GetAllGameFolders(value.Path),
            FolderKind.Uncategorized => GetUncategorizedGameFolders(value.Path),
            FolderKind.Category => GetCategoryGameFolders(value.Path),
            _ => Enumerable.Empty<string>()
        };

        foreach (var dir in gameDirs)
        {
            FilteredGames.Add(MakeGame(dir));
        }

        SelectedGame = FilteredGames.FirstOrDefault();
    }

    private static IEnumerable<string> GetUncategorizedGameFolders(string root)
    {
        foreach (var dir in Directory.GetDirectories(root))
            if (IsGameFolder(dir))
                yield return dir;
    }

    private static IEnumerable<string> GetCategoryGameFolders(string categoryDir)
    {
        foreach (var dir in Directory.GetDirectories(categoryDir))
        {
            if (IsGameFolder(dir))
            {
                yield return dir;
            }
        }
    }

    private static bool IsGameFolder(string dir)
    {
        return File.Exists(Path.Combine(dir, "_kobberlan.jpg"));
    }
    
    private static IEnumerable<string> GetAllGameFolders(string root)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var dir in Directory.GetDirectories(root))
        {
            if (IsGameFolder(dir) && seen.Add(dir))
                yield return dir;
        }

        foreach (var cat in Directory.GetDirectories(root))
        {
            if (IsGameFolder(cat))
            {
                continue;
            }
            
            foreach (var dir in Directory.GetDirectories(cat))
            {
                if (IsGameFolder(dir) && seen.Add(dir))
                    yield return dir;
            }
        }
    }
    
    private static Bitmap LoadFallbackCover()
    {
        var asm = typeof(App).Assembly.GetName().Name;
        var uri = new Uri($"avares://{asm}/Assets/covermissing.jpg");
        using var s = AssetLoader.Open(uri);
        return new Bitmap(s);
    }    

    private static Bitmap TryLoadCover(string gameDir)
    {
        var coverPath = Path.Combine(gameDir, "_kobberlan.jpg");

        if (File.Exists(coverPath))
        {
            try
            {
                using var s = File.OpenRead(coverPath);
                return new Bitmap(s);
            }
            catch
            {
                AppLog.Warn("Couldn't load cover, dir: " +  coverPath);
                return FallbackCover;
            }
        }

        return FallbackCover;
    }

    [RelayCommand]
    private void SelectGame(LocalGame game)
    {
        SelectedGame = game;
    }
}
