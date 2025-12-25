using Avalonia.Media.Imaging;

namespace KobberLan.Models;

public class GameCard
{
    public string Title { get; set; } = "";
    public Bitmap? Cover { get; set; }
    public int Likes { get; set; }
    public int Players { get; set; }
}