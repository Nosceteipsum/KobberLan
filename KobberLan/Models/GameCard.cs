using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KobberLan.Models;

public partial class GameCard : ObservableObject
{
    public string Title { get; set; } = "";
    public string Key { get; set; } = "";
    public string FolderPath { get; set; } = "";
    public Bitmap? Cover { get; set; }
    [ObservableProperty] private int likes;
    [ObservableProperty] private int players;
}