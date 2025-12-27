using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KobberLan.Models;

public partial class LocalGame : ObservableObject
{
    [ObservableProperty] private string key = "";
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string folderPath = "";
    [ObservableProperty] private Bitmap? cover;
}